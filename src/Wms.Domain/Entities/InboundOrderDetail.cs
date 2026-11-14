namespace Wms.Domain.Entities;

public class InboundOrderDetail
{
    protected InboundOrderDetail() { }

    private InboundOrderDetail(int materialId, int expectedQty)
    {
        if (materialId<=0)
            throw new ArgumentException("物料ID必须大于0");
        if (expectedQty <= 0)
            throw new ArgumentException("预期数量必须大于0");

        MaterialId = materialId;
        ExpectedQty = expectedQty;
        ReceivedQty = 0;

    }
    public static InboundOrderDetail Ceeate( int materialId, int expectedQty)
    {
        return new InboundOrderDetail(materialId, expectedQty);
    }
    public int Id { get; private set; }
    public int OrderId { get; private set; }
    public int MaterialId { get; private set; }
    public int ExpectedQty { get; private set; }
    public int ReceivedQty { get; private set; }
    public int? LocationId { get; private set; }
    public bool IsDeleted { get; private set; }

    public InboundOrder Order { get; private set; } = null!;
    public Material Material { get; private set; } = null!;
    public StorageLocation? Location { get; private set; }//StorageLocation可为null，因为入库单明细可能还没有分配到具体的库位

    public void Receive(int receivedQty, int locationId)
    {
        if (receivedQty<=0)
            throw new ArgumentException("实收数量必须大于0");
        if(receivedQty > ExpectedQty)
            throw new ArgumentException($"实收数量{receivedQty}不能大于预期数量{ExpectedQty}");
        if (locationId<0)
            throw new ArgumentException("上架库位ID必须大于0");

        ReceivedQty = receivedQty;
        LocationId = locationId;
    }
    public void SoftDelete()
    {
        IsDeleted = true;
    }
}