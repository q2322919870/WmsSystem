using Microsoft.EntityFrameworkCore;
using Wms.Domain.Entities;
using Wms.Domain.Exceptions;
using Wms.Domain.Interfaces;

namespace Wms.Infrastructure.Persistence.Repositories;

public class InventoryRepository:IInventoryRepository
{
    private readonly WmsDbContext _context;

    public InventoryRepository(WmsDbContext context)=> _context = context;

    public async Task AddAsync(Inventory inventory)=> await _context.Inventories.AddAsync(inventory);


    public async Task<List<Inventory>> GetAllWithDetailsAsync()
        => await _context.Inventories.Include(i => i.Warehouse)
            .Include(i => i.StorageLocation)
            .Include(i => i.Material)
            .AsNoTracking()
            .ToListAsync();
    // 收货要改库存，必须追踪
    public async Task<Inventory?> GetByLocationAndMaterialAsync(int locationId, int materialId)
        => await _context.Inventories.FirstOrDefaultAsync(i=>i.LocationId==locationId && i.MaterialId==materialId);
    

    public async Task<List<InventoryTransaction>> GetTransactionsByReferenceAsync(string referenceNo)
        => await _context.InventoryTransactions.Where(t=>t.ReferenceNo == referenceNo)
            .Include(t=>t.Warehouse)
            .Include(t=>t.Location)
            .Include(t=>t.Material)
            .AsNoTracking()
            .ToListAsync();
    
    public async Task AddTransactionAsync(InventoryTransaction transaction)
        => await _context.InventoryTransactions.AddAsync(transaction);

    public async Task SaveChangeAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // EF Core 的技术异常，在 Infrastructure 层转译成业务语言
            throw new BusinessException("库存已被其他操作修改，请刷新后重试");
            // 边界原则：EF Core 的异常类型不允许泄露到 Application 层，在 Repository 里转成 BusinessException
        }
    }
}