using Wms.Domain.Entities;

namespace Wms.Domain.Interfaces;

public interface IStocktakeOrderRepository
{
    Task AddAsync(StocktakeOrder order);
    Task<StocktakeOrder?> GetByIdForProcessingAsync(int orderId);// 用于提交盘点，需要追踪
    Task<List<StocktakeOrder>> GetAllWithDetailsAsync();
    Task SaveChangesAsync();
}