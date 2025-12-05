using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Enums;
using RestaurantOrders.Domain.Interfaces.Repositories;
using RestaurantOrders.Infrastructure.Persistence;

namespace RestaurantOrders.Infrastructure.Repositories
{
    internal class MenuItemRepository : IMenuItemRepository
    {
        private readonly AppDbContext _db;

        public MenuItemRepository(AppDbContext db) => _db = db;

        public async Task<MenuItem?> GetByIdAsync(Guid id)
        {
            return await _db.MenuItems.FindAsync(id);
        }

        public async Task<IReadOnlyList<MenuItem>> GetAllAsync(bool includeInactive = false)
        {
            var query = _db.MenuItems.AsQueryable();
            
            if (!includeInactive)
            {
                query = query.Where(m => m.IsActive);
            }

            return await query.OrderBy(m => m.Category).ThenBy(m => m.Name).ToListAsync();
        }

        public async Task<IReadOnlyList<MenuItem>> GetByCategoryAsync(MenuCategory category)
        {
            return await _db.MenuItems
                .Where(m => m.Category == category && m.IsActive)
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<MenuItem>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            var idList = ids.ToList();
            return await _db.MenuItems
                .Where(m => idList.Contains(m.Id))
                .ToListAsync();
        }

        public async Task<MenuItem> AddAsync(MenuItem item)
        {
            _db.MenuItems.Add(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task<MenuItem> UpdateAsync(MenuItem item)
        {
            _db.MenuItems.Update(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var item = await _db.MenuItems.FindAsync(id);
            if (item == null) return false;

            // Soft delete by setting IsActive to false
            item.IsActive = false;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
