using RestaurantOrders.Domain.Enums;
using RestaurantOrders.Domain.Exceptions;
using RestaurantOrders.Domain.Interfaces.Services;

namespace RestaurantOrders.Domain.Entities
{
    /// <summary>
    /// Represents a customer order. Aggregate Root.
    /// Contains business logic for adding items and calculating discounts.
    /// </summary>
    public class Order
    {
        private readonly List<OrderItem> _items = new();

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        
        public decimal Subtotal { get; private set; }
        public decimal DiscountApplied { get; private set; }
        public decimal Total { get; private set; }

        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        // Navigation property
        public User User { get; set; } = null!;

        /// <summary>
        /// Adds a menu item to the order, enforcing business rules.
        /// </summary>
        /// <exception cref="DuplicateSandwichInOrderException">When trying to add a second sandwich.</exception>
        /// <exception cref="DuplicateFriesInOrderException">When trying to add duplicate fries.</exception>
        /// <exception cref="DuplicateSoftDrinkInOrderException">When trying to add a duplicate soft drink.</exception>
        public void AddItem(MenuItem menuItem)
        {
            ArgumentNullException.ThrowIfNull(menuItem);

            ValidateItemCanBeAdded(menuItem);

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = Id,
                ItemId = menuItem.Id,
                ItemNameSnapshot = menuItem.Name,
                UnitPriceSnapshot = menuItem.BasePrice,
                CategorySnapshot = menuItem.Category,
                SubCategorySnapshot = menuItem.SubCategory,
                Quantity = 1
            };

            _items.Add(orderItem);
        }

        /// <summary>
        /// Removes an item from the order by its OrderItem Id.
        /// </summary>
        public bool RemoveItem(Guid orderItemId)
        {
            var item = _items.FirstOrDefault(i => i.Id == orderItemId);
            if (item == null) return false;
            
            _items.Remove(item);
            return true;
        }

        /// <summary>
        /// Removes an item from the order by its MenuItem Id.
        /// </summary>
        public bool RemoveItemByMenuItemId(Guid menuItemId)
        {
            var item = _items.FirstOrDefault(i => i.ItemId == menuItemId);
            if (item == null) return false;
            
            _items.Remove(item);
            return true;
        }

        /// <summary>
        /// Clears all items and replaces them with new ones.
        /// Used for order updates.
        /// </summary>
        public void ReplaceItems(IEnumerable<MenuItem> menuItems)
        {
            _items.Clear();
            foreach (var menuItem in menuItems)
            {
                AddItem(menuItem);
            }
        }

        /// <summary>
        /// Recalculates subtotal, discount, and total using the provided discount policy.
        /// </summary>
        public void RecalculateTotals(IDiscountPolicy discountPolicy)
        {
            Subtotal = _items.Sum(i => i.UnitPriceSnapshot * i.Quantity);
            DiscountApplied = discountPolicy.CalculateDiscount(_items);
            Total = Subtotal - DiscountApplied;
        }

        /// <summary>
        /// Validates that the item can be added without violating business rules.
        /// </summary>
        private void ValidateItemCanBeAdded(MenuItem menuItem)
        {
            // Rule: Only one sandwich per order
            if (menuItem.Category == MenuCategory.Sandwich)
            {
                if (_items.Any(i => i.CategorySnapshot == MenuCategory.Sandwich))
                {
                    throw new DuplicateSandwichInOrderException();
                }
            }

            // Rule: Only one fries per order
            if (menuItem.Category == MenuCategory.Extra && menuItem.SubCategory == MenuSubCategory.Fries)
            {
                if (_items.Any(i => i.CategorySnapshot == MenuCategory.Extra && 
                                    i.SubCategorySnapshot == MenuSubCategory.Fries))
                {
                    throw new DuplicateFriesInOrderException();
                }
            }

            // Rule: Only one soft drink per order
            if (menuItem.Category == MenuCategory.Extra && menuItem.SubCategory == MenuSubCategory.SoftDrink)
            {
                if (_items.Any(i => i.CategorySnapshot == MenuCategory.Extra && 
                                    i.SubCategorySnapshot == MenuSubCategory.SoftDrink))
                {
                    throw new DuplicateSoftDrinkInOrderException();
                }
            }
        }

        /// <summary>
        /// For EF Core to properly load existing items.
        /// </summary>
        public void SetItems(List<OrderItem> items)
        {
            _items.Clear();
            _items.AddRange(items);
        }
    }
}
