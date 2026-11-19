using Wms.Domain.Exceptions;

namespace Wms.Domain.Entities;

public class Inventory
{
    protected Inventory(){}

    private Inventory(int warehouseId, int locationId, int materialId)
    {
        if (warehouseId <= 0)
            throw new BusinessException("仓库ID必须大于0");
        if (locationId <= 0)
            throw new BusinessException("库位ID必须大于0");
        if (materialId <= 0)
            throw new BusinessException ("物料ID必须大于0");

        WarehouseId = warehouseId;
        LocationId = locationId;
        MaterialId = materialId;
        Qty = 0;
        LockedQty = 0;
    }
    public static Inventory Create(int warehouseId, int locationId, int materialId)
    {
        return new Inventory(warehouseId, locationId, materialId);
    }
    public int Id { get; private set; }
    public int WarehouseId { get; private set; }
    public int LocationId { get; private set; }
    public int MaterialId { get; private set; }
    public int Qty { get; private set; }
    public int LockedQty { get; private set; }
    
    // 计算属性：可用数量 = 总数量 - 锁定数量
    public int AvailableQty => Qty - LockedQty;

    public Warehouse Warehouse { get; private set; } = null!;
    public StorageLocation StorageLocation { get; private set; } = null!;
    public Material Material { get; private set; } = null!;
    public byte[] RowVersion { get; private set; } = [];

    public void Increase(int qty)
    {
        if (qty <= 0)
            throw new BusinessException("增加的数量必须大于0");
        Qty += qty;
    }
    // 行为方法：出库扣减库存（必须先锁定再扣减，这里简化处理）
    public void Decrease(int qty)
    {
        if (qty <= 0)
            throw new BusinessException("减少的数量必须大于0");
        if (qty > AvailableQty)
            throw new BusinessException($"可用库存不足，当前可用{AvailableQty}，需要{qty}");
        Qty -= qty;
        LockedQty -= qty;
    }

    public void Lock(int qty)
    {
        if (qty<=0)
            throw new BusinessException("锁定的数量必须大于0");
        if (qty > AvailableQty)
            throw new BusinessException($"可用库存不足，无法锁定，当前可用 {AvailableQty}，需要锁定 {qty}");
        LockedQty += qty;
    }
    public void UnLock(int qty)
    {
        if (qty<=0)
            throw new BusinessException("解锁的数量必须大于0");
        if (qty > LockedQty)
            throw new BusinessException($"解锁数量 {qty} 不能大于锁定数量 {LockedQty}");
        LockedQty -= qty;
    }

    // 盘点调整：直接设置为目标数量
    public void Adjust(int targetQty)
    {
        if(targetQty < 0)
            throw new BusinessException("调整后库存不能为负");
        Qty = targetQty;
        LockedQty = 0;// 盘点时通常要求无锁定，或者强制解锁
    }
}