using Wms.Domain.Entities;

namespace Wms.Application.DTOs;

public class CreateOutboundOrderDto
{
    public string OrderNo { get; set; } = string.Empty;
    public int WarehouseId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime RequireTime { get; set; }

    public List<CreateOutboundOrderDetailDto> Details { get; set; } = new();
    
}

public class CreateOutboundOrderDetailDto
{
    public int MaterialId { get; set; }
    public int RequiredQty { get; set; }
}