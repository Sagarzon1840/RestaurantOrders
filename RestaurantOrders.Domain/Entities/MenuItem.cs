using RestaurantOrders.Domain.Enums;

namespace RestaurantOrders.Domain.Entities
{
    /// <summary>
    /// Represents a menu item (sandwich or extra) in the restaurant.
    /// Aggregate Root.
    /// </summary>
    public class MenuItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public MenuCategory Category { get; set; }
        public MenuSubCategory SubCategory { get; set; } = MenuSubCategory.None;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
    }
}
