using Wms.Application.DTOs;
using Wms.Domain.Interfaces;

namespace Wms.Application.Services;

public class InventoryService
{
    private readonly IInventoryRepository _repository;

    public InventoryService(IInventoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<InventoryDto>> GetAllAsync()
    {
        var list = await _repository.GetAllWithDetailsAsync();
        return list.Select(i => new InventoryDto()
        {
            Id = i.Id,
            WarehouseId = i.WarehouseId,
            WarehouseName = i.Warehouse.Name,
            LocationId = i.LocationId,
            LocationCode = i.StorageLocation.Code,
            MaterialId = i.MaterialId,
            MaterialName = i.Material.Name,
            MaterialCode = i.Material.Code,
            Unit = i.Material.Unit,
            Qty = i.Qty,
            LockedQty = i.LockedQty,
            AvailableQty = i.AvailableQty
        }).ToList();
    }

    public async Task<List<InventoryTransactionDto>> GetTransactionsAsync(string referenceNo)
    {
        var list = await _repository.GetTransactionsByReferenceAsync(referenceNo);
        return list.Select(t=>new InventoryTransactionDto()
        {
            Id = t.Id,
            TransactionType = t.TransactionType,
            ReferenceNo = t.ReferenceNo,
            MaterialName = t.Material.Name,
            LocationCode = t.Location.Code,
            QtyChange = t.QtyChange,
            BeforeQty = t.BeforeQty,
            AfterQty = t.AfterQty,
            OperatorName = t.OperatorName,
            CreateTime = t.CreateTime
        }).ToList();
    } 
}