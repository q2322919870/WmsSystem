namespace Wms.Application.DTOs;

public class MaterialDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string BarCode { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int? ShelfLifeDays { get; set; }
}