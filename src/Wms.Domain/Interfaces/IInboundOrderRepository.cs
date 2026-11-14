using Wms.Domain.Entities;

namespace Wms.Domain.Interfaces;

public interface IInboundOrderRepository
{
    Task AddAsync(InboundOrder order);
    Task<bool> ExistsByCodeAsync(string orderNo);
    // 场景 A：用于列表展示或导出（需要完整关联数据，避免 N+1）
    Task<List<InboundOrder>> GetAllWithDetailsAsync();
    
    // 场景 B：用于执行收货业务（必须包含明细、物料、库位，否则无法校验和操作）
    Task<InboundOrder?> GetByIdForReceivingAsync(int id);
    
    // 场景 C：仅用于极轻量级的状态检查（比如只查单号判断是否存在，不需要任何关联数据）
    Task<InboundOrder?> GetBasicByIdAsync(int id);
    Task SaveChangesAsync();
}