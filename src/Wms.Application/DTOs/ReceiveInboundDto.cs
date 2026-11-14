namespace Wms.Application.DTOs;

public class ReceiveInboundDto
{
    public int DetailId { get; set; }
    public int ReceivedQty { get; set; }
    public int LocationId { get; set; }
    public string OperatorName { get; set; } = string.Empty;
}