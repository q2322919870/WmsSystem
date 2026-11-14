namespace Wms.Application.DTOs;

public class StorageLocationDto
{
    public int Id { get; set; }
    public int StorageAreaId { get; set; }
    public string StorageName { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int MaxVolume { get; set; }
    public int MaxWeight { get; set; }
    public string Status { get; set; } = string.Empty;
}