using Wms.Domain.Exceptions;

namespace Wms.Domain.Entities;

public class Warehouse
{
    protected Warehouse()
    {
    }

    private Warehouse(string code, string name, string address)
    {
        // 实体内部的格式校验（不依赖数据库）
        if (string.IsNullOrWhiteSpace(code))
            throw new BusinessException("仓库编码不能为空");
        if (code.Length > 50)
            throw new BusinessException("仓库编码长度不能超过50");
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessException("仓库名称不能为空");
        if (name.Length > 100)
            throw new BusinessException("仓库名称长度不能超过100");
        Code = code.Trim();
        Name = name.Trim();
        Address = address?.Trim() ?? string.Empty;
        CreateTime = DateTime.Now;
    }
    public static Warehouse Create(string code, string name, string address)
    {
        return new Warehouse(code, name, address);
    }
    public int Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; }= string.Empty;
    public string Address { get; private set; }= string.Empty;
    public DateTime CreateTime { get; private set; }
    public bool IsDeleted { get; set; }
    
    public void ChangeAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new BusinessException("仓库地址不能为空");
        }
        Address = address.Trim();
    }
    public void SoftDelete()
    {
        IsDeleted = true;
    }
}