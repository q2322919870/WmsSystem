using Microsoft.EntityFrameworkCore;
using Wms.Domain.Entities;
using Wms.Domain.Interfaces;

namespace Wms.Infrastructure.Persistence.Repositories;

public class OutboundOrderRepository:IOutboundOrderRepository
{
    private readonly WmsDbContext _context;

    public OutboundOrderRepository(WmsDbContext context)=>_context = context;

    public async Task AddAsync(OutboundOrder order)=>
        await _context.OutboundOrders.AddAsync(order);

    public async Task<bool> ExistsByOrderNoAsync(string orderNo)
        => await _context.OutboundOrders.AsNoTracking().AnyAsync(o => o.OrderNo == orderNo && !o.IsDeleted);


    public async Task<List<OutboundOrder>> GetAllWithDetailsAsync()
        => await _context.OutboundOrders.Where(o => !o.IsDeleted)
            .Include(o=>o.Warehouse)
            .Include(o=>o.Details).ThenInclude(d=>d.Material)
            .Include(o=>o.Details).ThenInclude(d=>d.Location)
            .AsNoTracking().ToListAsync();

    public async Task<OutboundOrder?> GetByIdForAllocationAsync(int id)
        => await _context.OutboundOrders
            .Include(o=>o.Warehouse)
            .Include(o => o.Details)
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);

    public async Task<OutboundOrder?> GetByIdForPickingAsync(int id)
        => await _context.OutboundOrders
            .Include(o=>o.Warehouse)
            .Include(o => o.Details)
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);

    public async Task<OutboundOrder?> GetByIdForShippingAsync(int id)
        => await _context.OutboundOrders
            .Include(o=>o.Warehouse)
            .Include(o => o.Details).ThenInclude(d => d.Material)
            .Include(o => o.Details).ThenInclude(d => d.Location)
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}