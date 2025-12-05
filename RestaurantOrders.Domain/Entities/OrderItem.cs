using RestaurantOrders.Domain.Enums;

namespace RestaurantOrders.Domain.Entities
{
    /// <summary>
    /// Represents an item within an order.
    /// Stores snapshot of price and name at time of purchase.
    /// </summary>
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        
        /// <summary>
        /// Name of the item at the time of purchase (snapshot).
        /// </summary>
        public string ItemNameSnapshot { get; set; } = null!;
        
        /// <summary>
        /// Price of the item at the time of purchase (snapshot).
        /// </summary>
        public decimal UnitPriceSnapshot { get; set; }
        
        /// <summary>
        /// Category at time of purchase for discount calculation.
        /// </summary>
        public MenuCategory CategorySnapshot { get; set; }
        
        /// <summary>
        /// SubCategory at time of purchase for discount calculation.
        /// </summary>
        public MenuSubCategory SubCategorySnapshot { get; set; }
        
        public int Quantity { get; set; } = 1;

        // Navigation properties
        public Order Order { get; set; } = null!;
        public MenuItem MenuItem { get; set; } = null!;
    }
}
