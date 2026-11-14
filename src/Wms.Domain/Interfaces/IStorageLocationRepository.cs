using Wms.Domain.Entities;

namespace Wms.Domain.Interfaces;

public interface IStorageLocationRepository
{
    Task AddAsync(StorageLocation storageLocation);
    Task<bool> ExistsByCodeInAreaAsync(int storageAreaId, string code);
    Task<List<StorageLocation>> GetAllAsync();
    Task<List<StorageLocation>> GetByAreaIdAsync(int storageAreaId);
    Task<StorageLocation?> GetByIdwithAreaAsync(int id);
    Task SaveChangesAsync();
}