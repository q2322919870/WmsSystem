using Microsoft.EntityFrameworkCore;
using Wms.Domain.Entities;
using Wms.Domain.Interfaces;

namespace Wms.Infrastructure.Persistence.Repositories;

public class MaterialRepository:IMaterialRepository
{
    private readonly WmsDbContext _context;

    public MaterialRepository(WmsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Material material)
    {
        await _context.Materials.AddAsync(material);
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _context.Materials.AsNoTracking().AnyAsync(m => m.Code == code && !m.IsDeleted);
    }

    public async Task<List<Material>> GetAllAsync()
    {
        return await _context.Materials.Where(m=>!m.IsDeleted).AsNoTracking().ToListAsync();
    }

    public async Task<Material?> GetByIdAsync(int id)
    {
        return await _context.Materials.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}