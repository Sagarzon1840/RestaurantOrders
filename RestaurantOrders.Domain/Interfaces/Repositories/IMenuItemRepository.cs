using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Enums;

namespace RestaurantOrders.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for MenuItem aggregate.
    /// </summary>
    public interface IMenuItemRepository
    {
        Task<MenuItem?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<MenuItem>> GetAllAsync(bool includeInactive = false);
        Task<IReadOnlyList<MenuItem>> GetByCategoryAsync(MenuCategory category);
        Task<IReadOnlyList<MenuItem>> GetByIdsAsync(IEnumerable<Guid> ids);
        Task<MenuItem> AddAsync(MenuItem item);
        Task<MenuItem> UpdateAsync(MenuItem item);
        Task<bool> DeleteAsync(Guid id);
    }
}
