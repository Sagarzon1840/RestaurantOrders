using RestaurantOrders.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace RestaurantOrders.Application.DTOs
{
    public class OrderResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal DiscountApplied { get; set; }
        public decimal Total { get; set; }
        public List<OrderItemResponseDto> Items { get; set; } = new();
    }

    public class OrderItemResponseDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? SubCategory { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateOrderDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "At least one item is required")]
        public List<Guid> ItemIds { get; set; } = new();
    }

    public class UpdateOrderDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "At least one item is required")]
        public List<Guid> ItemIds { get; set; } = new();

        public OrderStatus? Status { get; set; }
    }

    public class OrderSummaryDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public int ItemCount { get; set; }
    }
}
