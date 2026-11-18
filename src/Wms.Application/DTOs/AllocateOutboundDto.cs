namespace Wms.Application.DTOs;

public class AllocateOutboundDto
{
    public int DetailId { get; set; }
    public int LocationId { get; set; }
    public int AllocatedQty { get; set; }
}

public class PickOutboundDto
{
    public int DetailId { get; set; }
    public int PickedQty { get; set; }
}

public class CheckOutboundDto
{
    public int DetailId { get; set; }
}