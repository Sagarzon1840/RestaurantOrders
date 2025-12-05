using RestaurantOrders.Domain.Entities;

namespace RestaurantOrders.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Order aggregate.
    /// </summary>
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id);
        Task<Order?> GetByIdWithItemsAsync(Guid id);
        Task<IReadOnlyList<Order>> GetAllAsync();
        Task<IReadOnlyList<Order>> GetAllWithItemsAsync();
        Task<IReadOnlyList<Order>> GetByUserIdAsync(Guid userId);
        Task<IReadOnlyList<Order>> GetByUserIdWithItemsAsync(Guid userId);
        Task<Order> AddAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task<bool> DeleteAsync(Guid id);
    }
}
