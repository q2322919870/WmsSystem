using Wms.Domain.Entities;

namespace Wms.Domain.Interfaces;

public interface IStorageAreaRepository
{
    Task AddAsync(StorageArea storageArea);
    Task<bool> ExistsByCodeInWarehouseAsync(int warehouseId, string code);
    Task<List<StorageArea>> GetAllAsync();
    Task<List<StorageArea>> GetByWarehouseIdAsync(int warehouseId);
    Task SaveChangesAsync();
    
}