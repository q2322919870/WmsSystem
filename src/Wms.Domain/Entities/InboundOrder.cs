using Wms.Domain.Constants;
using Wms.Domain.Exceptions;

namespace Wms.Domain.Entities;

public class InboundOrder
{
    // 导航属性：一个入库单有多个明细
    private readonly List<InboundOrderDetail> _details = new();
    protected InboundOrder() { }

    private InboundOrder(string orderNo,int warehouseId, string supplierName, DateTime expectedTime)
    {
        if (string.IsNullOrWhiteSpace(orderNo))
            throw new BusinessException("入库单号不能为空");
        if (orderNo.Length > 50)
            throw new BusinessException("入库单号长度不能超过50");
        if (warehouseId <= 0)
            throw new BusinessException("收货仓库ID必须大于0");
        if (string.IsNullOrWhiteSpace(supplierName))
            throw new BusinessException("供应商名称不能为空");
        
        OrderNo = orderNo.Trim();
        WarehouseId = warehouseId;
        SupplierName = supplierName.Trim();
        ExpectedTime = expectedTime;
        Status = "待收货";
        CreateTime = DateTime.Now;
    }

    public static InboundOrder Create(string orderNo,int warehouseId, string supplierName, DateTime expectedTime)
    {
        return new InboundOrder(orderNo,warehouseId, supplierName, expectedTime);
    }
    public int Id { get; private set; }
    public string OrderNo { get; private set; } = string.Empty;
    public int WarehouseId { get; private set; }
    public string SupplierName { get; private set; } = string.Empty;
    public DateTime ExpectedTime { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public DateTime CreateTime { get; private set; }
    public bool IsDeleted { get; private set; }
    
    public Warehouse Warehouse { get; set; } = null!;
    public IReadOnlyCollection<InboundOrderDetail> Details => _details.AsReadOnly();
    // IReadOnlyCollection<InboundOrderDetail>类型的Details属性，表示入库单的明细集合，外部代码只能读取，不能修改集合内容

    public void AddDetail(InboundOrderDetail detail)
    {
        _details.Add(detail);
    }
    // 核心业务动作：收货。聚合根统一守护状态一致性
    public InboundOrderDetail ReceivedDetail(int detailId,int receivedQty,int locationId)
    {
        if (Status == InboundOrderStatus.Completed)
            throw new BusinessException("入库单已完成收货，不能继续收货");
        if (Status == InboundOrderStatus.Cancelled)
            throw new BusinessException("入库单已取消，不能继续收货");
        var detail = _details.FirstOrDefault(d=>d.Id == detailId&&!d.IsDeleted)
            ?? throw new BusinessException($"入库单明细{detailId}不存在或已删除");
        detail.Receive(receivedQty, locationId);
        RefreshStatus();
        return detail;
    }
    // 状态由明细进度推导，不允许外部随意设置
    private void RefreshStatus()
    {
        var activeDetails = _details.Where(d => !d.IsDeleted).ToList();
        if (activeDetails.Count > 0 && activeDetails.All(d=>d.IsFullyReceived))
            Status = InboundOrderStatus.Completed;
        else if (activeDetails.Any(d=>d.ReceivedQty > 0))
            Status = InboundOrderStatus.PartiallyReceived;
        else
            Status = InboundOrderStatus.Pending;
    }

    public void SoftDelete()
    {
        if (Status != InboundOrderStatus.Pending)
            throw new BusinessException("只有待收货状态的入库单才能删除");
        IsDeleted = true;
    }
}