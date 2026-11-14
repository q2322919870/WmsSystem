namespace Wms.Domain.Entities;

public class Material
{
    protected Material() { }
    private Material(string code, string name, string unit, string? barCode, string? category, int? shelfLifeDays)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("物料编码不能为空");
        if (code.Length>50)
            throw new ArgumentException("物料编码长度不能超过50");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("物料名称不能为空");

        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("物料单位不能为空");
        if (shelfLifeDays.HasValue && shelfLifeDays.Value < 0)
            throw new ArgumentException("保质期天数不能为负数");
        
        Code = code.Trim();
        Name = name.Trim();
        Unit = unit.Trim();
        BarCode = barCode?.Trim() ?? string.Empty;
        Category = category?.Trim() ?? string.Empty;
        ShelfLifeDays = shelfLifeDays;
        CreateTime = DateTime.Now;
    }

    public static Material Create(string code, string name, string unit, string? barCode =null, string? category =null,
        int? shelfLifeDays =null)
    {
        return new Material(code, name, unit, barCode, category, shelfLifeDays);
    }
    public int Id { get; private set; }
    public string Code { get; private set; } =string.Empty;
    public string Name { get; private set; } =string.Empty;
    public string Unit { get; private set; } = string.Empty;
    public string BarCode { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public int? ShelfLifeDays { get; private set; }
    public DateTime CreateTime { get; private set; }
    public bool IsDeleted { get; private set; }
    
    public void ChangeCategory(string newCategory)
    {
        Category = newCategory?.Trim() ?? string.Empty;
    }
    public void SoftDelete()
    {
        IsDeleted = true;
    }
}