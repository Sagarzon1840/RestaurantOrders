using RestaurantOrders.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace RestaurantOrders.Application.DTOs
{
    public class MenuItemResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? SubCategory { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateMenuItemDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 10000)]
        public decimal BasePrice { get; set; }

        [Required]
        public MenuCategory Category { get; set; }

        public MenuSubCategory SubCategory { get; set; } = MenuSubCategory.None;
    }

    public class UpdateMenuItemDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 10000)]
        public decimal BasePrice { get; set; }

        [Required]
        public MenuCategory Category { get; set; }

        public MenuSubCategory SubCategory { get; set; } = MenuSubCategory.None;

        public bool IsActive { get; set; } = true;
    }
}
