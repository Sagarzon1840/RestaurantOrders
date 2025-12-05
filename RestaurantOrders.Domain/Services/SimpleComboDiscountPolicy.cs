using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Enums;
using RestaurantOrders.Domain.Interfaces.Services;

namespace RestaurantOrders.Domain.Services
{
    /// <summary>
    /// Implements combo discount rules:
    /// - Sandwich + Fries + Soft Drink = 20% discount
    /// - Sandwich + Soft Drink = 15% discount
    /// - Sandwich + Fries = 10% discount
    /// </summary>
    public class SimpleComboDiscountPolicy : IDiscountPolicy
    {
        private const decimal FullComboDiscountRate = 0.20m;    // 20%
        private const decimal SandwichDrinkDiscountRate = 0.15m; // 15%
        private const decimal SandwichFriesDiscountRate = 0.10m; // 10%

        public decimal CalculateDiscount(IReadOnlyCollection<OrderItem> items)
        {
            if (items == null || items.Count == 0)
                return 0m;

            bool hasSandwich = items.Any(i => i.CategorySnapshot == MenuCategory.Sandwich);
            bool hasFries = items.Any(i => i.CategorySnapshot == MenuCategory.Extra && 
                                          i.SubCategorySnapshot == MenuSubCategory.Fries);
            bool hasSoftDrink = items.Any(i => i.CategorySnapshot == MenuCategory.Extra && 
                                               i.SubCategorySnapshot == MenuSubCategory.SoftDrink);

            decimal subtotal = items.Sum(i => i.UnitPriceSnapshot * i.Quantity);

            // Rule 1: Full combo (sandwich + fries + soft drink) = 20% discount
            if (hasSandwich && hasFries && hasSoftDrink)
            {
                return subtotal * FullComboDiscountRate;
            }

            // Rule 2: Sandwich + soft drink = 15% discount
            if (hasSandwich && hasSoftDrink)
            {
                return subtotal * SandwichDrinkDiscountRate;
            }

            // Rule 3: Sandwich + fries = 10% discount
            if (hasSandwich && hasFries)
            {
                return subtotal * SandwichFriesDiscountRate;
            }

            // No combo discount applies
            return 0m;
        }
    }
}
