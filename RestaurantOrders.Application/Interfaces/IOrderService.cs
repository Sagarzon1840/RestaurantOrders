using RestaurantOrders.Application.DTOs;

namespace RestaurantOrders.Application.Interfaces
{
    /// <summary>
    /// Application service for order operations.
    /// </summary>
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(Guid userId, CreateOrderDto dto);
        Task<OrderResponseDto?> GetByIdAsync(Guid orderId, Guid userId, bool isAdmin);
        Task<IReadOnlyList<OrderResponseDto>> GetAllAsync(Guid? userId, bool isAdmin);
        Task<IReadOnlyList<OrderResponseDto>> GetByUserIdAsync(Guid userId);
        Task<OrderResponseDto?> UpdateOrderAsync(Guid orderId, UpdateOrderDto dto, Guid userId, bool isAdmin);
        Task<bool> DeleteOrderAsync(Guid orderId, Guid userId, bool isAdmin);
    }
}
