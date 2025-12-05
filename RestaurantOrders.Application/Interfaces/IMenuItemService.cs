using RestaurantOrders.Application.DTOs;
using RestaurantOrders.Domain.Enums;

namespace RestaurantOrders.Application.Interfaces
{
    /// <summary>
    /// Application service for menu item operations.
    /// </summary>
    public interface IMenuItemService
    {
        Task<IReadOnlyList<MenuItemResponseDto>> GetAllAsync();
        Task<IReadOnlyList<MenuItemResponseDto>> GetSandwichesAsync();
        Task<IReadOnlyList<MenuItemResponseDto>> GetExtrasAsync();
        Task<MenuItemResponseDto?> GetByIdAsync(Guid id);
        Task<MenuItemResponseDto> CreateAsync(CreateMenuItemDto dto);
        Task<MenuItemResponseDto?> UpdateAsync(Guid id, UpdateMenuItemDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
