using Wms.Domain.Exceptions;

namespace Wms.Domain.Entities;

public class StocktakeOrder
{
    protected StocktakeOrder(){}

    private StocktakeOrder(string orderNo, int warehousId, string operatorName)
    {
        if(string.IsNullOrWhiteSpace(orderNo))
            throw new BusinessException("盘点单号不能为空");
        if(warehousId <= 0)
            throw new BusinessException("仓库ID必须大于0");
        if(string.IsNullOrWhiteSpace(operatorName))
            throw new BusinessException("操作人不能为空");
        OrderNo = orderNo.Trim();
        WarehousId = warehousId;
        OperatorName = operatorName.Trim();
        Status = "待盘点";
        CreateTime = DateTime.Now;
    }
    public static StocktakeOrder Create(string orderNo, int warehousId, string operatorName)
        => new StocktakeOrder(orderNo, warehousId, operatorName);
    public int Id { get; private set; }
    public string OrderNo { get; private set; }
    public int WarehousId { get; private set; }
    public string OperatorName { get; private set; }
    public string Status { get; private set; }
    public DateTime CreateTime { get; private set; }
    public bool IsDeleted { get; private set; }
    private List<StocktakeOrderDetail> _details = new();
    public IReadOnlyCollection<StocktakeOrderDetail> Details => _details;

    public Warehouse Warehouse { get; private set; } = null!;
    
    public void AddDetail(StocktakeOrderDetail detail)
    {
        if(Status != "待盘点")
            throw new BusinessException($"只有待盘点状态的单据才能添加明细");
        _details.Add(detail);
    }

    public void Submit()
    {
        if (Status != "待盘点")
            throw new BusinessException("只有待盘点状态的单据才能提交");
        if (!_details.Any())
            throw new BusinessException("盘点单不能没有明细");
        Status = "已完成";// MVP 简化：提交即完成并调整库存

    }
}