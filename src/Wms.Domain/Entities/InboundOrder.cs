namespace Wms.Domain.Entities;

public class InboundOrder
{
    protected InboundOrder() { }

    private InboundOrder(string orderNo, string supplierName, DateTime expectedTime)
    {
        if (string.IsNullOrWhiteSpace(orderNo))
            throw new ArgumentException("入库单号不能为空");
        if (orderNo.Length > 50)
            throw new ArgumentException("入库单号长度不能超过50");
        if (string.IsNullOrWhiteSpace(supplierName))
            throw new ArgumentException("供应商名称不能为空");
        
        OrderNo = orderNo.Trim();
        SupplierName = supplierName.Trim();
        ExpectedTime = expectedTime;
        Status = "待收货";
        CreateTime = DateTime.Now;
    }

    public static InboundOrder Create(string orderNo, string SupplierName, DateTime expectedTime)
    {
        return new InboundOrder(orderNo, SupplierName, expectedTime);
    }
    public int Id { get; private set; }
    public string OrderNo { get; private set; } = string.Empty;
    public string SupplierName { get; private set; } = string.Empty;
    public DateTime ExpectedTime { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public DateTime CreateTime { get; private set; }
    public bool IsDeleted { get; private set; }
    // 导航属性：一个入库单有多个明细
    private readonly List<InboundOrderDetail> _details = new();
    public IReadOnlyCollection<InboundOrderDetail> Details => _details.AsReadOnly();
    // IReadOnlyCollection<InboundOrderDetail>类型的Details属性，表示入库单的明细集合，外部代码只能读取，不能修改集合内容

    public void AddDetail(InboundOrderDetail detail)
    {
        _details.Add(detail);
    }
    public void ChangeStatus(string newStatus)
    {
        if (string.IsNullOrWhiteSpace(newStatus))
            throw new ArgumentException("状态不能为空");
        Status = newStatus.Trim();
    }
    public void SoftDelete()
    {
        IsDeleted = true;
    }
}