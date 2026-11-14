using Wms.Application.DTOs;
using Wms.Domain.Entities;
using Wms.Domain.Exceptions;
using Wms.Domain.Interfaces;

namespace Wms.Application.Services;

public class InboundOrderService
{
    private readonly IInboundOrderRepository _repository;
    private readonly IMaterialRepository _materialRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IStorageLocationRepository _locationRepository;
    private readonly IInventoryRepository _inventoryRepository;

    public InboundOrderService(IInboundOrderRepository repository, IMaterialRepository materialRepository, IWarehouseRepository warehouseRepository, IStorageLocationRepository locationRepository, IInventoryRepository inventoryRepository)
    {
        _repository = repository;
        _materialRepository = materialRepository;
        _warehouseRepository = warehouseRepository;
        _locationRepository = locationRepository;
        _inventoryRepository = inventoryRepository;
    }

    public async Task<bool> CreateAsync(CreateInboundOrderDto dto)
    {
        if (await _repository.ExistsByCodeAsync(dto.OrderNo))
            throw new BusinessException($"入库单号{dto.OrderNo}已存在");
        if (dto.Details.Count == 0)
            throw new BusinessException("入库单至少需要一条明细");
        var warehouses = await _warehouseRepository.GetAllAsync();
        if (warehouses.All(w=>w.Id!=dto.WarehouseId))
            throw new BusinessException($"仓库ID{dto.WarehouseId}不存在");
        foreach (var detail in dto.Details)
        {
            var material = await _materialRepository.GetByIdAsync(detail.MaterialId);
            if(material is null)
                throw new BusinessException($"物料ID{detail.MaterialId}不存在");
        }
        var order = InboundOrder.Create(dto.OrderNo, dto.WarehouseId, dto.SupplierName, dto.ExpectedTime);
        foreach (var detailDto in dto.Details)
            order.AddDetail(InboundOrderDetail.Ceeate(detailDto.MaterialId, detailDto.ExpectedQty));
        
        await _repository.AddAsync(order);
        await _repository.SaveChangesAsync();
        return true;
    }
    // ⭐ 收货：一次操作改 4 张表，全程在一个事务内
    public async Task<bool> ReceiveAsync(int orderId, ReceiveInboundDto dto)
    {
        // 1. 取出聚合（追踪状态）
        var order = await _repository.GetByIdForReceivingAsync(orderId)
                    ?? throw new BusinessException($"入库单ID{orderId}不存在");
        // 2. 校验库位存在，且属于入库单指定的仓库
        var location = await _locationRepository.GetByIdwithAreaAsync(dto.LocationId)
            ?? throw new BusinessException($"库位ID{dto.LocationId}不存在");
        if (location.StorageArea.WarehouseId != order.WarehouseId)
            throw new BusinessException($"库位{location.Code}不属于入库单{order.OrderNo}指定的仓库，不能上架");
        if (string.IsNullOrWhiteSpace(dto.OperatorName))
            throw new BusinessException("操作人不能为空");
        // 3. 领域行为：改明细数量 + 推导单据状态（规则都在实体里）
        var detail = order.ReceivedDetail(dto.DetailId, dto.ReceivedQty, dto.LocationId);
        // 4. 库存 Upsert：有则累加，无则新建
        var inventory = await _inventoryRepository.GetByLocationAndMaterialAsync(dto.LocationId, detail.MaterialId);
        var beforeQty = inventory?.Qty ?? 0;
        if (inventory is null)
        {
            inventory = Inventory.Create(order.WarehouseId, dto.LocationId, detail.MaterialId);
            await _inventoryRepository.AddAsync(inventory);
        }
        inventory.Increase(dto.ReceivedQty);
        // 5. 写库存流水（账实相符的凭据，只增不改）
        var transaction = InventoryTransaction.Create(
            transactionType: "入库",
            referenceNo: order.OrderNo,
            warehouseId: order.WarehouseId,
            locationId: dto.LocationId,
            materialId: detail.MaterialId,
            qtyChange: dto.ReceivedQty,
            beforeQty: beforeQty,
            afterQty: inventory.Qty,
            operatorName: dto.OperatorName.Trim());
        await _inventoryRepository.AddTransactionAsync(transaction);
        // 6. 一次提交 = 一个数据库事务
        await _inventoryRepository.SaveChangeAsync();
        return true;
    }
    public async Task<List<InboundOrderDto>> GetAllAsync()
    {
        var order = await _repository.GetAllWithDetailsAsync();
        return order.Select(o=>new InboundOrderDto()
        {
            Id = o.Id,
            OrderNo = o.OrderNo,
            WarehouseId = o.WarehouseId,
            SupplierName = o.SupplierName,
            ExpectedTime = o.ExpectedTime,
            Status = o.Status,
            Details = o.Details.Select(d=>new InboundOrderDetailDto()
            {
                Id = d.Id,
                MaterialId = d.MaterialId,
                MaterialName = d.Material?.Name?? "未知物料",
                ExpectedQty = d.ExpectedQty,
                ReceivedQty = d.ReceivedQty,
                LocationId = d.LocationId,
                LocationName = d.Location?.Name?? "未上架"
            }).ToList()
        }).ToList();
    }   
    public async Task<InboundOrderDto?> GetByIdForReceivingAsync(int id)
    {
        var order = await _repository.GetByIdForReceivingAsync(id);
        if (order is null)
            return null;
        return new InboundOrderDto()
        {
            Id = order.Id,
            OrderNo = order.OrderNo,
            WarehouseId = order.WarehouseId,
            SupplierName = order.SupplierName,
            ExpectedTime = order.ExpectedTime,
            Status = order.Status,
            Details = order.Details.Select(d=>new InboundOrderDetailDto()
            {
                Id = d.Id,
                MaterialId = d.MaterialId,
                MaterialName = d.Material?.Name?? "未知物料",
                ExpectedQty = d.ExpectedQty,
                ReceivedQty = d.ReceivedQty,
                LocationId = d.LocationId,
                LocationName = d.Location?.Name?? "未上架"
            }).ToList()
        };
    }
    public async Task<InboundOrderDto?> GetBasicByIdAsync(int id)
    {
        var order = await _repository.GetBasicByIdAsync(id);
        if (order is null)
            return null;
        return new InboundOrderDto()
        {
            Id = order.Id,
            OrderNo = order.OrderNo,
            WarehouseId = order.WarehouseId,
            SupplierName = order.SupplierName,
            ExpectedTime = order.ExpectedTime,
            Status = order.Status,
            Details = order.Details.Select(d=>new InboundOrderDetailDto()
            {
                Id = d.Id,
                MaterialId = d.MaterialId,
                MaterialName = d.Material?.Name?? "未知物料",
                ExpectedQty = d.ExpectedQty,
                ReceivedQty = d.ReceivedQty,
                LocationId = d.LocationId,
                LocationName = d.Location?.Name?? "未上架"
            }).ToList()
        };
    }
}