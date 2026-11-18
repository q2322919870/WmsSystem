using Wms.Domain.Entities;

namespace Wms.Domain.Interfaces;

public interface IOutboundOrderRepository
{
    Task AddAsync(OutboundOrder order);
    Task<bool> ExistsByOrderNoAsync(string orderNo);
    Task<List<OutboundOrder>> GetAllWithDetailsAsync();
    Task<OutboundOrder?> GetByIdForAllocationAsync(int id);
    Task<OutboundOrder?> GetByIdForPickingAsync(int id);
    Task<OutboundOrder?> GetByIdForShippingAsync(int id);
    Task SaveChangesAsync();
}