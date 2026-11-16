namespace Wms.Application.DTOs;

public class OutboundOrderDto
{
    public int Id { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public int WarehousId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime RequiredTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<OutboundOrderDetailDto> Type { get; set; }
}

public class OutboundOrderDetailDto
{
    public int Id { get; set; }
    public int MaterialId { get; set; }
    public string MaterialName { get; set; } = string.Empty;
    public int RequiredQty { get; set; }
    public int AllocatedQty { get; set; }
    public int PickedQty { get; set; }
    public int? LocationId { get; set; }
    public string LocationCode { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    
}