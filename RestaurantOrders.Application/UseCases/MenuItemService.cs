using RestaurantOrders.Application.DTOs;
using RestaurantOrders.Application.Interfaces;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Enums;
using RestaurantOrders.Domain.Interfaces.Repositories;

namespace RestaurantOrders.Application.UseCases
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public MenuItemService(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        public async Task<IReadOnlyList<MenuItemResponseDto>> GetAllAsync()
        {
            var items = await _menuItemRepository.GetAllAsync();
            return items.Select(MapToDto).ToList();
        }

        public async Task<IReadOnlyList<MenuItemResponseDto>> GetSandwichesAsync()
        {
            var items = await _menuItemRepository.GetByCategoryAsync(MenuCategory.Sandwich);
            return items.Select(MapToDto).ToList();
        }

        public async Task<IReadOnlyList<MenuItemResponseDto>> GetExtrasAsync()
        {
            var items = await _menuItemRepository.GetByCategoryAsync(MenuCategory.Extra);
            return items.Select(MapToDto).ToList();
        }

        public async Task<MenuItemResponseDto?> GetByIdAsync(Guid id)
        {
            var item = await _menuItemRepository.GetByIdAsync(id);
            return item != null ? MapToDto(item) : null;
        }

        public async Task<MenuItemResponseDto> CreateAsync(CreateMenuItemDto dto)
        {
            var menuItem = new MenuItem
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                BasePrice = dto.BasePrice,
                Category = dto.Category,
                SubCategory = dto.SubCategory,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _menuItemRepository.AddAsync(menuItem);
            return MapToDto(created);
        }

        public async Task<MenuItemResponseDto?> UpdateAsync(Guid id, UpdateMenuItemDto dto)
        {
            var existing = await _menuItemRepository.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Name = dto.Name.Trim();
            existing.BasePrice = dto.BasePrice;
            existing.Category = dto.Category;
            existing.SubCategory = dto.SubCategory;
            existing.IsActive = dto.IsActive;

            var updated = await _menuItemRepository.UpdateAsync(existing);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _menuItemRepository.DeleteAsync(id);
        }

        private static MenuItemResponseDto MapToDto(MenuItem item) => new()
        {
            Id = item.Id,
            Name = item.Name,
            BasePrice = item.BasePrice,
            Category = item.Category.ToString(),
            SubCategory = item.SubCategory != MenuSubCategory.None ? item.SubCategory.ToString() : null,
            IsActive = item.IsActive,
            CreatedAt = item.CreatedAt
        };
    }
}
