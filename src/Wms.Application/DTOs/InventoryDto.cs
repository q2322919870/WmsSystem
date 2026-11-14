namespace Wms.Application.DTOs;

public class InventoryDto
{
    public int Id { get; set; }
    public int WarehouseId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
    public int LocationId { get; set; }
    public string LocationCode { get; set; } = string.Empty;
    public int MaterialId { get; set; }
    public string MaterialCode { get; set; } = string.Empty;
    public string MaterialName { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int Qty { get; set; }
    public int LockedQty { get; set; }
    public int AvailableQty { get; set; }
}

public class InventoryTransactionDto
{
    public int Id { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public string ReferenceNo { get; set; } = string.Empty;
    public string MaterialName { get; set; } = string.Empty;
    public string LocationCode { get; set; } = string.Empty;
    public int QtyChange { get; set; }
    public int BeforeQty { get; set; }
    public int AfterQty { get; set; }
    public string OperatorName { get; set; } = string.Empty;
    public DateTime CreateTime { get; set; }
}