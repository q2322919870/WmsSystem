using Wms.Domain.Entities;

namespace Wms.Domain.Interfaces;

public interface IMaterialRepository
{
    Task AddAsync(Material material);
    Task<bool> ExistsByCodeAsync(string code);
    Task<List<Material>> GetAllAsync();
    Task<Material?> GetByIdAsync(int id);
    Task SaveChangesAsync();
}