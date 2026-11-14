using Microsoft.EntityFrameworkCore;
using Wms.Domain.Entities;
using Wms.Domain.Interfaces;

namespace Wms.Infrastructure.Persistence.Repositories;

public class StorageAreaRepository:IStorageAreaRepository
{
    private readonly WmsDbContext _context;

    public StorageAreaRepository(WmsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(StorageArea storageArea)
    {
        await _context.StorageAreas.AddAsync(storageArea);
    }

    public async Task<bool> ExistsByCodeInWarehouseAsync(int warehouseId, string code)
    {
        return await _context.StorageAreas
            .AsNoTracking()
            .AnyAsync(a=>a.WarehouseId==warehouseId && a.Code==code && !a.IsDeleted);
    }

    public async Task<List<StorageArea>> GetAllAsync()
    {
        return await _context.StorageAreas.Where(a=>!a.IsDeleted).AsNoTracking().ToListAsync();
    }

    public async Task<List<StorageArea>> GetByWarehouseIdAsync(int warehouseId)
    {
        return await _context.StorageAreas.Where(a=>a.WarehouseId==warehouseId && !a.IsDeleted)
            .AsNoTracking().ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}