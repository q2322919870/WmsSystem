namespace Wms.Application.DTOs;

public class InboundOrderDto
{
    public int Id { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public DateTime ExpectedTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<InboundOrderDetailDto> Details { get; set; } = new();
}

public class InboundOrderDetailDto
{
    public int Id { get; set; }
    public int MaterialId { get; set; }
    public string MaterialName { get; set; } = string.Empty;
    public int ExpectedQty { get; set; }
    public int ReceivedQty { get; set; }
    public int? LocationId { get; set; }
    public string LocationName { get; set; } = string.Empty;
}