namespace Wms.Application.DTOs;

public class CreateStorageLocationDto
{
    public int StorageAreaId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int MaxVolume { get; set; }
    public int MaxWeight { get; set; }
}