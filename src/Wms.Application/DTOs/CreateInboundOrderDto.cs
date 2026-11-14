namespace Wms.Application.DTOs;

public class CreateInboundOrderDto
{
    public string OrderNo { get; set; } = string.Empty;
    public int WarehouseId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public DateTime ExpectedTime { get; set; }
    public List<CreateInboundOrderDetailDto> Details { get; set; } = new ();
}

public class CreateInboundOrderDetailDto
{
    public int MaterialId { get; set; }
    public int ExpectedQty { get; set; }    
}