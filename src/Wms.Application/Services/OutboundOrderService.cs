using System.Xml;
using Wms.Application.DTOs;
using Wms.Domain.Entities;
using Wms.Domain.Exceptions;
using Wms.Domain.Interfaces;

namespace Wms.Application.Services;

public class OutboundOrderService
{
    private readonly IOutboundOrderRepository _repository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly IStorageLocationRepository _locationRepository;
    private readonly IInventoryRepository _inventoryRepository;

    public OutboundOrderService(IOutboundOrderRepository repository, IWarehouseRepository warehouseRepository, IMaterialRepository materialRepository, IStorageLocationRepository locationRepository, IInventoryRepository inventoryRepository)
    {
        _repository = repository;
        _warehouseRepository = warehouseRepository;
        _materialRepository = materialRepository;
        _locationRepository = locationRepository;
        _inventoryRepository = inventoryRepository;
    }
    // 1. 创建出库单
    public async Task<bool> CreateOutboundOrderAsync(CreateOutboundOrderDto dto)
    {
        if (await _repository.ExistsByOrderNoAsync(dto.OrderNo))
            throw new BusinessException($"出库单号 {dto.OrderNo} 已存在");
        if (dto.Details.Count is 0)
            throw new BusinessException("出库单至少需要一条明细");
        var warehouses = await _warehouseRepository.GetAllAsync();
        if (warehouses.All(w=>w.Id != dto.WarehouseId))
            throw new BusinessException($"仓库Id {dto.WarehouseId} 不存在");

        foreach (var detailDto in dto.Details)
        {
            if (await _materialRepository.GetByIdAsync(detailDto.MaterialId) is null)
                throw new BusinessException($"物料Id {detailDto.MaterialId} 不存在");
        }
        var order = OutboundOrder.Create(dto.OrderNo, dto.WarehouseId, dto.CustomerName, dto.RequireTime);
        foreach (var detail in dto.Details)
            order.AddDetail(OutboundOrderDetail.Create(detail.MaterialId, detail.RequiredQty));
        await _repository.AddAsync(order);
        await _repository.SaveChangesAsync();
        return true;
    }
    // 2. 分配库存（锁定）⭐ 核心
    public async Task<bool> AllocateAsync(int orderId, AllocateOutboundDto dto)
    {
        var order = await _repository.GetByIdForAllocationAsync(orderId)
                    ?? throw new BusinessException($"出库单 {orderId} 不存在");
        var location = await _locationRepository.GetByIdWithAreaAsync(dto.LocationId)
            ?? throw new BusinessException($"库位 {dto.LocationId} 不存在");
        if (order.WarehouseId != location.StorageArea.WarehouseId)
            throw new BusinessException($"库位 {location.Code} 不属于出库单指定的仓库");
        var inventory = await _inventoryRepository.GetByLocationAndMaterialAsync(dto.LocationId,
            order.Details.First(d => d.Id == dto.DetailId).MaterialId);
        if(inventory == null || inventory.Qty <= dto.AllocatedQty)
            throw new BusinessException($"库位 {location.Code} 上的可用库存不足，无法分配");
        // 领域行为：更新明细状态  
        order.AllocateDetail(dto.DetailId,dto.LocationId,dto.AllocatedQty);
        // 核心：锁定库存
        inventory.Lock(dto.AllocatedQty);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> PickAsync(int orderId, PickOutboundDto dto)
    {
        var order = await _repository.GetByIdForAllocationAsync(orderId)
                    ?? throw new BusinessException($"出库单 {orderId} 不存在");
        order.PickDetail(dto.DetailId, dto.PickedQty);
        await _repository.SaveChangesAsync();
        return true;
    }
    // 4. 复核
    public async Task<bool> CheckAsync(int orderId, CheckOutboundDto dto)
    {
        var order = await _repository.GetByIdForAllocationAsync(orderId)
                    ?? throw new BusinessException($"出库单 {orderId} 不存在");
        order.CheckDetail(dto.DetailId);
        await _repository.SaveChangesAsync();
        return true;
    }
    // 5. 发货（扣减库存）⭐ 核心
    public async Task<bool> ShipAsync(int orderId, int detailId, string operatorName)
    {
        var order = await _repository.GetByIdForShippingAsync(orderId)
                    ?? throw new BusinessException($"出库单 {orderId} 不存在");
        if (string.IsNullOrWhiteSpace(operatorName))
            throw new BusinessException("操作人不能为空");
        // 领域行为：更新明细状态为已发货，并返回明细对象
        var detail = order.ShipDetail(detailId);
        // 核心：扣减库存
        var inventory =
            await _inventoryRepository.GetByLocationAndMaterialAsync(detail.LocationId!.Value, detail.MaterialId);
        if (inventory is null)
            throw new BusinessException("库存记录不存在，无法发货");
        inventory.Decrease(detail.PickedQty);
        // 写库存流水
        var transaction = InventoryTransaction.Create(
            transactionType: "出库",
            referenceNo: order.OrderNo,
            warehouseId: order.WarehouseId,
            materialId: detail.MaterialId,
            locationId: detail.LocationId!.Value,
            qtyChange: -detail.PickedQty, // 注意这里是负数！
            beforeQty: inventory.Qty + detail.PickedQty, // 扣减前的库存数量
            afterQty: inventory.Qty, // 扣减后的库存数量
            operatorName: operatorName
        );
        await _inventoryRepository.AddTransactionAsync(transaction);
        await _repository.SaveChangesAsync();
        return true;
    }
    // 6. 查询所有
    public async Task<List<OutboundOrderDto>> GetAllAsync()
    {
        var orders = await _repository.GetAllWithDetailsAsync();
        return orders.Select(o => new OutboundOrderDto
        {
            Id = o.Id,
            OrderNo = o.OrderNo,
            WarehouseId = o.WarehouseId,
            WarehouseName = o.Warehouse?.Name ?? "未知仓库",
            CustomerName = o.CustomerName,
            RequiredTime = o.RequireTime,
            Status = o.Status,
            Details = o.Details.Select(d => new OutboundOrderDetailDto
            {
                Id = d.Id,
                MaterialId = d.MaterialId,
                MaterialName = d.Material?.Name ?? "未知物料",
                RequiredQty = d.RequiredQty,
                AllocatedQty = d.AllocatedQty,
                PickedQty = d.PickedQty,
                LocationId = d.LocationId,
                LocationCode = d.Location?.Code ?? "未分配",
                Status = d.Status
            }).ToList()
        }).ToList();
    }
}