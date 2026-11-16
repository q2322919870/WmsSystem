using Wms.Domain.Exceptions;

namespace Wms.Domain.Entities;

public class OutboundOrder
{
    private List<OutboundOrderDetail> _details = new ();
    protected OutboundOrder(){}

    private OutboundOrder(string orderNo,int warehouseId,string customerName,DateTime requireTime)
    {
        if (string.IsNullOrWhiteSpace(orderNo))
            throw new BusinessException("订单号不能为空");
        if (orderNo.Length > 50)
            throw new BusinessException("订单号长度不能超过50");
        if (warehouseId <= 0)
            throw new BusinessException("仓库Id必须大于0");
        if (string.IsNullOrWhiteSpace(customerName))
            throw new BusinessException("客户名称不能为空");
        OrderNo = orderNo;
        WarehouseId = warehouseId;
        CustomerName = customerName;
        RequireTime = requireTime;
        Status = "待分配";
        CreateTime = DateTime.Now;
    }

    public static OutboundOrder Create(string orderNo, int warehouseId, string customerName, DateTime requireTime)
        => new OutboundOrder(orderNo, warehouseId, customerName, requireTime);
    
    public int Id { get; private set; }
    public string OrderNo { get; private set; } = string.Empty;
    public int WarehouseId { get; private set; }
    public string CustomerName { get; private set; } = string.Empty;
    public DateTime RequireTime { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public DateTime CreateTime { get; private set; }
    public bool IsDeleted { get; private set; }

    public Warehouse Warehouse { get; private set; }
    public IReadOnlyCollection<OutboundOrderDetail> Details => _details.AsReadOnly();

    public void AddDetail(OutboundOrderDetail detail)
    {
        if (Status is not "待分配")
            throw new BusinessException("只有待分配状态的出库单才能添加明细");
        if (_details.Any(d=>d.MaterialId == detail.MaterialId && !d.IsDeleted))
            throw new BusinessException($"物料{detail.MaterialId}已存在明细中，请勿重复添加");
        
        _details.Add(detail);
    }
    // 核心业务动作：分配库存（锁定）
    public void AllocateDetail(int detailId, int locationId, int allocatedQty)
    {
        if (Status is not "待分配"&& Status is not "已分配")
            throw new BusinessException("只有在待分配或已分配状态的出库单才能分配库存");
        var detail = _details.FirstOrDefault(d=>d.Id == detailId && !d.IsDeleted)
                     ?? throw new BusinessException($"订单明细{detailId}不存在");
        detail.Allocate(locationId, allocatedQty);
        RefreshStatus();
    }
    // 核心业务动作：拣货确认
    public void PickDetail(int detailId, int pickedQty)
    {
        if (Status is not "待分配")
            throw new BusinessException("只有在待分配状态的出库单才能拣货");
        var detail = _details.FirstOrDefault(d => d.Id == detailId && !d.IsDeleted)
                     ?? throw new BusinessException($"订单明细{detailId}不存在");
        detail.Pick(pickedQty);
        RefreshStatus();
    }
    // 核心业务动作：复核
    public void CheckDetail(int detailId)
    {
        if (Status is not "已拣货")
            throw new BusinessException("只有在已拣货状态的出库单才能复核");
        var detail = _details.FirstOrDefault(d=>d.Id == detailId && !d.IsDeleted)
                     ?? throw new BusinessException($"订单明细{detailId}不存在");
        detail.Check();
        
        RefreshStatus();
    }
    // 核心业务动作：发货（扣减库存）
    public OutboundOrderDetail ShipDetail(int detailId)
    {
        if (Status is not "已复核")
            throw new BusinessException("只有在已复核状态的出库单才能发货");
        var detail = _details.FirstOrDefault(d=>d.Id == detailId && !d.IsDeleted)
            ?? throw new BusinessException("订单明细不存在");
        detail.Ship();
        RefreshStatus();
        return detail;
    }

    private void RefreshStatus()
    {
        var activeDetails = _details.Where(d => !d.IsDeleted).ToList();
        if (activeDetails.Count is 0)
        {
            Status = "待分配";
            return;
        }

        if (activeDetails.All(d => d.Status == "已发货"))
            Status = "已发货";
        else if (activeDetails.All(d => d.Status == "已复核"))
            Status = "已复核";
        else if(activeDetails.All(d => d.Status == "已拣货"))
            Status = "已拣货";
        else if(activeDetails.All(d => d.Status == "已分配"))
            Status = "已分配";
        else
            Status = "待分配";
    }
    public void SoftDelete()
    {
        if (Status is not "待分配")
            throw new BusinessException("只有在待分配状态的出库单才能删除");
        IsDeleted = true;
    }
}