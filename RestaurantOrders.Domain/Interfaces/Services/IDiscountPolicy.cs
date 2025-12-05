using RestaurantOrders.Domain.Entities;

namespace RestaurantOrders.Domain.Interfaces.Services
{
    /// <summary>
    /// Domain service interface for calculating order discounts.
    /// </summary>
    public interface IDiscountPolicy
    {
        /// <summary>
        /// Calculates the discount amount for the given order items.
        /// </summary>
        /// <param name="items">The items in the order.</param>
        /// <returns>The discount amount to subtract from the subtotal.</returns>
        decimal CalculateDiscount(IReadOnlyCollection<OrderItem> items);
    }
}
