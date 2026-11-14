using Wms.Domain.Exceptions;

namespace Wms.Domain.Entities;

public class InboundOrderDetail
{
    protected InboundOrderDetail() { }

    private InboundOrderDetail(int materialId, int expectedQty)
    {
        if (materialId<=0)
            throw new BusinessException("物料ID必须大于0");
        if (expectedQty <= 0)
            throw new BusinessException("预期数量必须大于0");

        MaterialId = materialId;
        ExpectedQty = expectedQty;
        ReceivedQty = 0;

    }
    public static InboundOrderDetail Ceeate( int materialId, int expectedQty)=>
        new InboundOrderDetail(materialId, expectedQty);
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
    public bool IsFullyReceived => ReceivedQty >= ExpectedQty;

    public void Receive(int receivedQty, int locationId)
    {
        if (receivedQty<=0)
            throw new BusinessException("本次收货数量必须大于0");
        if (locationId<0)
            throw new BusinessException("上架库位ID必须大于0");
        var total = receivedQty + ReceivedQty;
        if (total > ExpectedQty)
            throw new BusinessException($"累计收货数量 {total} 超过预期数量 {ExpectedQty}，本次最多可收 {ExpectedQty - ReceivedQty}");
        if (LocationId.HasValue && LocationId.Value != locationId)
            throw new BusinessException($"该明细已上架至库位 {LocationId.Value}，不能混放；如需分库位存放请拆分入库明细");
        ReceivedQty = total;
        LocationId = locationId;
    }
    public void SoftDelete()
    {
        IsDeleted = true;
    }
}