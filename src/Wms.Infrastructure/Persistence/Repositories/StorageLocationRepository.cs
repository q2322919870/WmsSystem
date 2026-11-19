using Microsoft.EntityFrameworkCore;
using Wms.Domain.Entities;
using Wms.Domain.Interfaces;

namespace Wms.Infrastructure.Persistence.Repositories;

public class StorageLocationRepository:IStorageLocationRepository
{
    private readonly WmsDbContext _context;

    public StorageLocationRepository(WmsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(StorageLocation storageLocation)
    {
        await _context.StorageLocations.AddAsync(storageLocation);
    }

    public async Task<bool> ExistsByCodeInAreaAsync(int storageAreaId, string code)
    {
        return await _context.StorageLocations
            .AsNoTracking()
            .AnyAsync(l => l.StorageAreaId == storageAreaId && l.Code == code && !l.IsDeleted);
    }

    public async Task<List<StorageLocation>> GetAllAsync()
    {
        return await _context.StorageLocations.Where(l=>!l.IsDeleted).AsNoTracking().ToListAsync();
    }

    public async Task<List<StorageLocation>> GetByAreaIdAsync(int storageAreaId)
    {
        return await _context.StorageLocations
            .Where(l=>!l.IsDeleted && l.StorageAreaId == storageAreaId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<StorageLocation?> GetByIdWithAreaAsync(int id)
    {
        return await _context.StorageLocations.Include(l=>l.StorageArea)
            .AsNoTracking()
            .FirstOrDefaultAsync(l=>l.Id == id&& !l.IsDeleted);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}