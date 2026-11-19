using Wms.Application.DTOs;
using Wms.Domain.Entities;
using Wms.Domain.Exceptions;
using Wms.Domain.Interfaces;

namespace Wms.Application.Services;

public class StocktakeOrderService
{
    private readonly IStocktakeOrderRepository _repository;
    private readonly IStorageLocationRepository _locationRepository;
    private readonly IInventoryRepository _inventoryRepository;

    public StocktakeOrderService(IStocktakeOrderRepository repository, IStorageLocationRepository locationRepository, IInventoryRepository inventoryRepository)
    {
        _repository = repository;
        _locationRepository = locationRepository;
        _inventoryRepository = inventoryRepository;
    }

    public async Task<bool> CreateAsync(CreateStocktakeOrderDto dto)
    {
        var order = StocktakeOrder.Create(dto.OrderNo, dto.WarehouseId, dto.OperatorName);
        foreach (var item in dto.Items)
        {
            var location = await _locationRepository.GetByIdWithAreaAsync(item.LocationId) ??
                throw new BusinessException($"库位 {item.LocationId} 不存在");
            if (location.StorageArea.WarehouseId != dto.WarehouseId)
                throw new BusinessException($"库位 {location.Code} 不属于指定仓库");
            // 获取当前系统库存作为快照
            var inventory = await _inventoryRepository.GetByLocationAndMaterialAsync(item.LocationId, item.MaterialId);
            int systemQty = inventory?.Qty ?? 0;
            var detail = StocktakeOrderDetail.Create(item.LocationId, item.MaterialId, systemQty);
            detail.RecordActualQty(item.ActualQty);// 创建时直接录入实盘（MVP简化）
            order.AddDetail(detail);
        }
        await _repository.AddAsync(order);
        await _repository.SaveChangesAsync();
        return true;
    }
    // 2. 提交盘点（调整库存 + 写流水）
    public async Task<bool> SubmitAsync(int orderId, string reviewerName)
    {
        var order = await _repository.GetByIdForProcessingAsync(orderId)
            ?? throw new BusinessException($"盘点单 {orderId} 不存在");
        if(string.IsNullOrWhiteSpace(reviewerName))
            throw new BusinessException("审核人不能为空");
        order.Submit();
        foreach (var detail in order.Details)
        {
            if (detail.DifferenceQty is 0) continue;
            var inventory = await _inventoryRepository.GetByLocationAndMaterialAsync(detail.LocationId, detail.MaterialId);
            int beforeQty = inventory?.Qty ?? 0;
            int afterQty = beforeQty - detail.DifferenceQty;// 盘亏(Diff>0)则减，盘盈(Diff<0)则加
            if (inventory is null)
            {
                // 盘盈且原本无库存：新建库存
                inventory = Inventory.Create(order.WarehousId, detail.LocationId, detail.MaterialId);
                await _inventoryRepository.AddAsync(inventory);
            }
            // 直接设置数量（因为是盘点调整，不是增减）
            // 注意：这里需要给 Inventory 加一个 SetQty 方法，或者用 Increase/Decrease 的变体。
            // 为了简单，我们在 Inventory 里加个 Adjust 方法。
            inventory.Adjust(afterQty);
            var transactionType = detail.DifferenceQty > 0 ? "盘亏" : "盘盈";
            var transaction = InventoryTransaction.Create(
                transactionType,
                order.OrderNo,
                order.WarehousId,
                detail.LocationId,
                detail.MaterialId,
                -detail.DifferenceQty, // 盘亏是正数，盘盈是负数
                beforeQty,
                afterQty,
                reviewerName.Trim()
            );
            await _inventoryRepository.AddTransactionAsync(transaction);
        }
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<List<StocktakeOrderDto>> GetAllAsync()
    {
        var orders = await _repository.GetAllWithDetailsAsync();
        return orders.Select(o => new StocktakeOrderDto
        {
            Id = o.Id,
            OrderNo = o.OrderNo,
            WarehouseName = o.Warehouse?.Name??"未知",
            Status = o.Status,
            Details = o.Details.Select(d => new StocktakeOrderDetailDto
            {
                LocationCode = d.Location?.Code??"未知",
                MaterialName = d.Material?.Name??"未知",
                SystemQty = d.SystemQty,
                ActualQty = d.ActualQty,
                DifferenceQty = d.DifferenceQty
            }).ToList()
        }).ToList();
    }
}