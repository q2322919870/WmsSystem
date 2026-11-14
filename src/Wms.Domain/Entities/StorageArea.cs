namespace Wms.Domain.Entities;

public class StorageArea
{
    protected StorageArea(){}

    private StorageArea(int warehouseId, string code, string name, string type)
    {
        if (warehouseId <= 0)
            throw new ArgumentException("仓库ID必须大于0");
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("库区编码不能为空");
        if (code.Length > 50)
            throw new ArgumentException("库区编码长度不能超过50");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("库区名称不能为空");
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("库区类型不能为空");
        WarehouseId = warehouseId;
        Code = code.Trim();
        Name = name.Trim();
        Type = type.Trim();
        CreateTime = DateTime.Now;
        
    }

    public static StorageArea Create(int warehouseId, string code, string name, string type)
    {
        return new StorageArea(warehouseId, code, name, type);
    }

    public int Id { get; private set; }
    public int WarehouseId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public DateTime CreateTime { get; private set; }
    public bool IsDeleted { get; private set; }
    // 导航属性（EF Core 用，表示这个库区属于哪个仓库）
    public Warehouse Warehouse { get; set; } = null!;
}