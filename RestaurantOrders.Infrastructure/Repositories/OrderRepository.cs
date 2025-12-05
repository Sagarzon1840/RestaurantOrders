using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Interfaces.Repositories;
using RestaurantOrders.Infrastructure.Persistence;

namespace RestaurantOrders.Infrastructure.Repositories
{
    internal class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _db;

        public OrderRepository(AppDbContext db) => _db = db;

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _db.Orders.FindAsync(id);
        }

        public async Task<Order?> GetByIdWithItemsAsync(Guid id)
        {
            var order = await _db.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order != null)
            {
                // Load items into the aggregate
                order.SetItems(order.Items.ToList());
            }

            return order;
        }

        public async Task<IReadOnlyList<Order>> GetAllAsync()
        {
            return await _db.Orders
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Order>> GetAllWithItemsAsync()
        {
            var orders = await _db.Orders
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            foreach (var order in orders)
            {
                order.SetItems(order.Items.ToList());
            }

            return orders;
        }

        public async Task<IReadOnlyList<Order>> GetByUserIdAsync(Guid userId)
        {
            return await _db.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Order>> GetByUserIdWithItemsAsync(Guid userId)
        {
            var orders = await _db.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            foreach (var order in orders)
            {
                order.SetItems(order.Items.ToList());
            }

            return orders;
        }

        public async Task<Order> AddAsync(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            // Get existing items to handle removal
            var existingItems = await _db.OrderItems
                .Where(oi => oi.OrderId == order.Id)
                .ToListAsync();

            // Remove items that are no longer in the order
            var currentItemIds = order.Items.Select(i => i.Id).ToHashSet();
            var itemsToRemove = existingItems.Where(ei => !currentItemIds.Contains(ei.Id)).ToList();
            _db.OrderItems.RemoveRange(itemsToRemove);

            // Add new items
            var existingItemIds = existingItems.Select(ei => ei.Id).ToHashSet();
            var itemsToAdd = order.Items.Where(i => !existingItemIds.Contains(i.Id)).ToList();
            _db.OrderItems.AddRange(itemsToAdd);

            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var order = await _db.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
            
            if (order == null) return false;

            _db.OrderItems.RemoveRange(order.Items);
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
