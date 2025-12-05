using RestaurantOrders.Application.DTOs;
using RestaurantOrders.Application.Interfaces;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Enums;
using RestaurantOrders.Domain.Interfaces.Repositories;
using RestaurantOrders.Domain.Interfaces.Services;

namespace RestaurantOrders.Application.UseCases
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IDiscountPolicy _discountPolicy;

        public OrderService(
            IOrderRepository orderRepository,
            IMenuItemRepository menuItemRepository,
            IDiscountPolicy discountPolicy)
        {
            _orderRepository = orderRepository;
            _menuItemRepository = menuItemRepository;
            _discountPolicy = discountPolicy;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(Guid userId, CreateOrderDto dto)
        {
            if (dto.ItemIds == null || dto.ItemIds.Count == 0)
            {
                throw new ArgumentException("At least one item is required to create an order.");
            }

            // Get unique IDs to query the database
            var uniqueIds = dto.ItemIds.Distinct().ToList();
            
            // Get all menu items by unique IDs
            var menuItems = await _menuItemRepository.GetByIdsAsync(uniqueIds);
            var menuItemsDict = menuItems.ToDictionary(m => m.Id);
            
            // Check if all requested IDs exist
            var missingIds = uniqueIds.Where(id => !menuItemsDict.ContainsKey(id)).ToList();
            if (missingIds.Count > 0)
            {
                throw new ArgumentException($"Some menu items were not found: {string.Join(", ", missingIds)}");
            }

            // Verify all items are active
            var inactiveItems = menuItems.Where(m => !m.IsActive).ToList();
            if (inactiveItems.Count != 0)
            {
                throw new ArgumentException($"Some menu items are not available: {string.Join(", ", inactiveItems.Select(m => m.Name))}");
            }

            // Create order and add items
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            // Iterate over original list to respect duplicates and let Order validate them
            foreach (var itemId in dto.ItemIds)
            {
                var menuItem = menuItemsDict[itemId];
                order.AddItem(menuItem); // This validates duplicates
            }

            order.RecalculateTotals(_discountPolicy);

            var created = await _orderRepository.AddAsync(order);
            return MapToDto(created);
        }

        public async Task<OrderResponseDto?> GetByIdAsync(Guid orderId, Guid userId, bool isAdmin)
        {
            var order = await _orderRepository.GetByIdWithItemsAsync(orderId);
            if (order == null) return null;

            // Authorization check
            if (!isAdmin && order.UserId != userId)
            {
                return null; // Return null to indicate not found/not authorized
            }

            return MapToDto(order);
        }

        public async Task<IReadOnlyList<OrderResponseDto>> GetAllAsync(Guid? userId, bool isAdmin)
        {
            IReadOnlyList<Order> orders;

            if (isAdmin)
            {
                orders = await _orderRepository.GetAllWithItemsAsync();
            }
            else if (userId.HasValue)
            {
                orders = await _orderRepository.GetByUserIdWithItemsAsync(userId.Value);
            }
            else
            {
                return new List<OrderResponseDto>();
            }

            return orders.Select(MapToDto).ToList();
        }

        public async Task<IReadOnlyList<OrderResponseDto>> GetByUserIdAsync(Guid userId)
        {
            var orders = await _orderRepository.GetByUserIdWithItemsAsync(userId);
            return orders.Select(MapToDto).ToList();
        }

        public async Task<OrderResponseDto?> UpdateOrderAsync(Guid orderId, UpdateOrderDto dto, Guid userId, bool isAdmin)
        {
            var order = await _orderRepository.GetByIdWithItemsAsync(orderId);
            if (order == null) return null;

            // Authorization check
            if (!isAdmin && order.UserId != userId)
            {
                return null;
            }

            // Update status if provided
            if (dto.Status.HasValue)
            {
                order.Status = dto.Status.Value;
            }

            // Update items if provided
            if (dto.ItemIds != null && dto.ItemIds.Count > 0)
            {
                // Get unique IDs to query the database
                var uniqueIds = dto.ItemIds.Distinct().ToList();
                
                var menuItems = await _menuItemRepository.GetByIdsAsync(uniqueIds);
                var menuItemsDict = menuItems.ToDictionary(m => m.Id);
                
                // Check if all requested IDs exist
                var missingIds = uniqueIds.Where(id => !menuItemsDict.ContainsKey(id)).ToList();
                if (missingIds.Count > 0)
                {
                    throw new ArgumentException($"Some menu items were not found: {string.Join(", ", missingIds)}");
                }

                var inactiveItems = menuItems.Where(m => !m.IsActive).ToList();
                if (inactiveItems.Count != 0)
                {
                    throw new ArgumentException($"Some menu items are not available: {string.Join(", ", inactiveItems.Select(m => m.Name))}");
                }

                // Build list respecting order of original request (for duplicate validation)
                var orderedMenuItems = dto.ItemIds.Select(id => menuItemsDict[id]).ToList();
                order.ReplaceItems(orderedMenuItems);
                order.RecalculateTotals(_discountPolicy);
            }

            var updated = await _orderRepository.UpdateAsync(order);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteOrderAsync(Guid orderId, Guid userId, bool isAdmin)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return false;

            // Authorization check
            if (!isAdmin && order.UserId != userId)
            {
                return false;
            }

            return await _orderRepository.DeleteAsync(orderId);
        }

        private static OrderResponseDto MapToDto(Order order) => new()
        {
            Id = order.Id,
            UserId = order.UserId,
            CreatedAt = order.CreatedAt,
            Status = order.Status.ToString(),
            Subtotal = order.Subtotal,
            DiscountApplied = order.DiscountApplied,
            Total = order.Total,
            Items = order.Items.Select(i => new OrderItemResponseDto
            {
                Id = i.Id,
                ItemId = i.ItemId,
                ItemName = i.ItemNameSnapshot,
                UnitPrice = i.UnitPriceSnapshot,
                Category = i.CategorySnapshot.ToString(),
                SubCategory = i.SubCategorySnapshot != MenuSubCategory.None ? i.SubCategorySnapshot.ToString() : null,
                Quantity = i.Quantity
            }).ToList()
        };
    }
}
