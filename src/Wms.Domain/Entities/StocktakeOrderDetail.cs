using Wms.Domain.Exceptions;

namespace Wms.Domain.Entities;

public class StocktakeOrderDetail
{
    protected StocktakeOrderDetail() { }

    private StocktakeOrderDetail(int locationId, int materialId, int systemQty)
    {
        if(locationId<=0)
            throw new BusinessException("库位ID必须大于0");
        if(materialId<=0)
            throw new BusinessException("物料ID必须大于0");
        if(systemQty < 0)
            throw new BusinessException("系统库存数量不能为负数");
        LocationId = locationId;
        MaterialId = materialId;
        SystemQty = systemQty;
        ActualQty = 0;
        DifferenceQty = 0;
    }
    public static StocktakeOrderDetail Create(int locationId, int materialId, int systemQty) 
        => new StocktakeOrderDetail(locationId, materialId, systemQty);
    public int Id { get; private set; }
    public int OrderId { get; private set; }// 外键
    public int LocationId { get; private set; }
    public int MaterialId { get; private set; }
    public int SystemQty { get; private set; }// 系统库存数量
    public int ActualQty { get; private set; }// 实盘数量
    public int DifferenceQty { get; private set; }// 差异数量 = 实盘数量 - 系统库存数量
    public bool IsDeleted { get; private set; }
    
    public StocktakeOrder Order { get; private set; } = null!;
    public StorageLocation Location { get; private set; } = null!;
    public Material Material { get; private set; } = null!;

    public void RecordActualQty(int actualQty)
    {
        if (actualQty < 0)
            throw new BusinessException("实盘数量不能为负数");
        ActualQty = actualQty;
        DifferenceQty = SystemQty - ActualQty;// 正数代表盘亏，负数代表盘盈
    }
}