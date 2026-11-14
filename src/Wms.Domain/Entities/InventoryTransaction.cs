using Wms.Domain.Exceptions;

namespace Wms.Domain.Entities;

public class InventoryTransaction
{
    protected InventoryTransaction(){}

    private InventoryTransaction(string transactionType,string referenceNo,int warehouseId,int locationId,
    int materialId,int qtyChange,int beforeQty,int afterQty,string operatorName)
    {
        if (string.IsNullOrWhiteSpace(transactionType))
            throw new BusinessException("事务类型不能为空");
        if (string.IsNullOrWhiteSpace(referenceNo))
            throw new BusinessException("关联单号不能为空");
        if (warehouseId <= 0)
            throw new BusinessException("仓库ID必须大于0");
        if (locationId <= 0)
            throw new BusinessException("库位ID必须大于0");
        if (materialId <= 0)
            throw new BusinessException("物料ID必须大于0");
        if (qtyChange == 0)
            throw new BusinessException("数量变化不能为0");
        if (string.IsNullOrWhiteSpace(operatorName))
            throw new BusinessException("操作员姓名不能为空");
        TransactionType = transactionType;
        ReferenceNo = referenceNo;
        WarehouseId = warehouseId;
        LocationId = locationId;
        MaterialId = materialId;
        QtyChange = qtyChange;
        BeforeQty = beforeQty;
        AfterQty = afterQty;
        OperatorName = operatorName;
        CreateTime = DateTime.Now;
    }

    public static InventoryTransaction Create(string transactionType, string referenceNo, int warehouseId,
        int locationId, int materialId, int qtyChange, int beforeQty, int afterQty, string operatorName)
    {
        return new InventoryTransaction(transactionType, referenceNo, warehouseId, locationId, materialId, qtyChange, beforeQty, afterQty, operatorName);
    }
    public int Id { get; private set; }
    public string TransactionType { get; private set; } = string.Empty;
    public string ReferenceNo { get; private set; } = string.Empty;
    public int WarehouseId { get; private set; }
    public int LocationId { get; private set; }
    public int MaterialId { get; private set; }
    public int QtyChange { get; private set; }
    public int BeforeQty { get; private set; }
    public int AfterQty { get; private set; }
    public string OperatorName { get; private set; } = string.Empty;
    public DateTime CreateTime { get; private set; }

    public Warehouse Warehouse { get; private set; } = null!;
    public StorageLocation Location { get; private set; } = null!;
    public Material Material { get; private set; } = null!;
}