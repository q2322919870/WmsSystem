using Wms.Domain.Entities;

namespace Wms.Domain.Interfaces;

public interface IWarehouseRepository
{
    Task AddAsync(Warehouse warehouse);
    Task<bool> ExistsAsync(string code);
    Task<List<Warehouse>> GetAllAsync();
    Task SaveChangesAsync();
}