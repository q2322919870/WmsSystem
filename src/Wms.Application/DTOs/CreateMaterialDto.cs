namespace Wms.Application.DTOs;

public class CreateMaterialDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string? BarCode { get; set; }
    public string? Category { get; set; }
    public int? ShelfLifeDays { get; set; }
}