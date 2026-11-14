using Microsoft.EntityFrameworkCore;
using Wms.Domain.Entities;
using Wms.Domain.Interfaces;

namespace Wms.Infrastructure.Persistence.Repositories;

public class WarehouseRepository:IWarehouseRepository
{
    private readonly WmsDbContext _context;
    public WarehouseRepository(WmsDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Warehouse warehouse)
    {
        await _context.Warehouses.AddAsync(warehouse);
    }

    public async Task<bool> ExistsAsync(string code)
    {
        return await _context.Warehouses.AsNoTracking()
            .AnyAsync(w=>w.Code==code && !w.IsDeleted);//asNoTracking()方法用于查询不跟踪实体的更改，适用于只读操作，可以提高性能。
    }

    public async Task<List<Warehouse>> GetAllAsync()
    {
        return await _context.Warehouses.Where(w => !w.IsDeleted).AsNoTracking().ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}