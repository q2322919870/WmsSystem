using Microsoft.EntityFrameworkCore;
using Wms.Domain.Entities;
using Wms.Domain.Interfaces;

namespace Wms.Infrastructure.Persistence.Repositories;

public class InboundOrderRepository:IInboundOrderRepository
{
    private readonly WmsDbContext _context;

    public InboundOrderRepository(WmsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(InboundOrder order)
    {
        await _context.InboundOrders.AddAsync(order);
    }

    public async Task<bool> ExistsByCodeAsync(string orderNo)
    {
        return await _context.InboundOrders.AsNoTracking().AnyAsync(o => o.OrderNo == orderNo && !o.IsDeleted);
    }

    public async Task<List<InboundOrder>> GetAllWithDetailsAsync()
    {
        return await _context.InboundOrders
            .Where(o=>!o.IsDeleted)
            .Include(o=>o.Details)
                .ThenInclude(d=>d.Material)
            .Include(o=>o.Details)
                .ThenInclude(d=>d.Location)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<InboundOrder?> GetByIdForReceivingAsync(int id)
    {
        return await _context.InboundOrders
            .Include(o => o.Details)
                .ThenInclude(d => d.Material)
            .Include(o => o.Details)
                .ThenInclude(d => d.Location)
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
    }

    public async Task<InboundOrder?> GetBasicByIdAsync(int id)
    {
        return await _context.InboundOrders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}