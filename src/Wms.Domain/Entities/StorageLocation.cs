namespace Wms.Domain.Entities;

public class StorageLocation
{
    protected StorageLocation(){}

    private StorageLocation(int storageAreaId, string code, string name, int maxVolume, int maxWeight)
    {
        if (storageAreaId <= 0)
            throw new ArgumentException("库区ID必须大于0");
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("库位编码不能为空");
        if (code.Length > 50)
            throw new ArgumentException("库位编码长度不能超过50");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("库位名称不能为空");
        if (maxVolume < 0)
            throw new ArgumentException("最大容积不能为负数");
        if (maxWeight < 0)
            throw new ArgumentException("最大承重不能为负数");
        StorageAreaId = storageAreaId;
        Code = code.Trim();
        Name = name.Trim();
        MaxVolume = maxVolume;
        MaxWeight = maxWeight;
        Status = "空闲";
        CreateTime = DateTime.Now;
    }

    public static StorageLocation Create(int storageAreaId, string code, string name, int maxVolume, int maxWeight)
    {
        return new StorageLocation(storageAreaId, code, name, maxVolume, maxWeight);
    }

    public int Id { get; private set; }
    public int StorageAreaId { get;private set; }
    public string Code { get;private set; } = string.Empty;
    public string Name { get;private set; } = string.Empty;
    public int MaxVolume { get; private set; }
    public int MaxWeight { get; private set; }
    public string Status { get;private set; } = string.Empty;
    public DateTime CreateTime { get;private set; }
    public bool IsDeleted { get; private set; }

    public StorageArea StorageArea { get; private set; } = null!;// Navigation property

    public void ChangeStatus(string newStatus)
    {
        if (string.IsNullOrWhiteSpace(newStatus))
        {
            throw new ArgumentException("状态不能为空");
        }
        Status = newStatus.Trim();
    }
    public void SoftDelete()
    {
        IsDeleted = true;
    }
}