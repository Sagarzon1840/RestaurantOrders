using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOrders.Application.DTOs;
using RestaurantOrders.Application.Interfaces;

namespace RestaurantOrders.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        /// <summary>
        /// Gets all menu items.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IReadOnlyList<MenuItemResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var items = await _menuItemService.GetAllAsync();
            return Ok(items);
        }

        /// <summary>
        /// Gets only sandwiches.
        /// </summary>
        [HttpGet("sandwiches")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IReadOnlyList<MenuItemResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSandwiches()
        {
            var items = await _menuItemService.GetSandwichesAsync();
            return Ok(items);
        }

        /// <summary>
        /// Gets only extras (fries, soft drinks).
        /// </summary>
        [HttpGet("extras")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IReadOnlyList<MenuItemResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExtras()
        {
            var items = await _menuItemService.GetExtrasAsync();
            return Ok(items);
        }

        /// <summary>
        /// Gets a specific menu item by ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MenuItemResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _menuItemService.GetByIdAsync(id);
            
            if (item == null)
            {
                return NotFound(new { error = "Menu item not found" });
            }

            return Ok(item);
        }

        /// <summary>
        /// Creates a new menu item (Admin only).
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(MenuItemResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateMenuItemDto dto)
        {
            var created = await _menuItemService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing menu item (Admin only).
        /// </summary>
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(MenuItemResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMenuItemDto dto)
        {
            var updated = await _menuItemService.UpdateAsync(id, dto);
            
            if (updated == null)
            {
                return NotFound(new { error = "Menu item not found" });
            }

            return Ok(updated);
        }

        /// <summary>
        /// Deactivates a menu item (Admin only).
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _menuItemService.DeleteAsync(id);
            
            if (!result)
            {
                return NotFound(new { error = "Menu item not found" });
            }

            return NoContent();
        }
    }
}
