using Wms.Domain.Entities;

namespace Wms.Domain.Interfaces;

public interface IInventoryRepository
{
    Task AddAsync(Inventory inventory);
    Task<List<Inventory>> GetAllWithDetailsAsync();
    Task<Inventory?> GetByLocationAndMaterialAsync(int locationId, int materialId);
    Task<List<InventoryTransaction>> GetTransactionsByReferenceAsync(string referenceNo);
    Task AddTransactionAsync(InventoryTransaction transaction);
    Task SaveChangeAsync();
}