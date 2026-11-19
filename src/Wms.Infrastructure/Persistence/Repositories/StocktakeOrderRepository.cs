using Microsoft.EntityFrameworkCore;
using Wms.Domain.Entities;
using Wms.Domain.Interfaces;

namespace Wms.Infrastructure.Persistence.Repositories;

public class StocktakeOrderRepository: IStocktakeOrderRepository
{
    private readonly WmsDbContext _context;

    public StocktakeOrderRepository(WmsDbContext context)
    => _context = context;

    public async Task AddAsync(StocktakeOrder order)
        => await _context.StocktakeOrders.AddAsync(order);

    public async Task<StocktakeOrder?> GetByIdForProcessingAsync(int id)
    => await _context.StocktakeOrders
        .Include(o=>o.Details)
            .ThenInclude(d=>d.Material)
        .Include(o=>o.Details)
            .ThenInclude(d=>d.Location)
        .FirstOrDefaultAsync(o=>o.Id == id&& !o.IsDeleted);

    public async Task<List<StocktakeOrder>> GetAllWithDetailsAsync()
    => await _context.StocktakeOrders.Include(o=>o.Warehouse)
        .Include(o=>o.Details)
            .ThenInclude(d=>d.Material)
        .Include(o=>o.Details)
            .ThenInclude(d=>d.Location)
        .Where(o=>!o.IsDeleted)
        .AsNoTracking()
        .ToListAsync();

    public async Task SaveChangesAsync()=> await _context.SaveChangesAsync();
}