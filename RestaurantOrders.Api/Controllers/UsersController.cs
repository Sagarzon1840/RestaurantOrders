using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOrders.Api.Models;
using RestaurantOrders.Application.DTOs;
using RestaurantOrders.Application.Interfaces;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Enums;

namespace RestaurantOrders.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly ICurrentUserService _currentUserService;

        public UsersController(
            IUserService userService, 
            IOrderService orderService,
            ICurrentUserService currentUserService)
        {
            _userService = userService;
            _orderService = orderService;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Creates a new user (Admin only).
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] UserRegisterDTO dto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = dto.Username,
                Name = dto.Name,
                Role = UserRole.User
            };

            var success = await _userService.UserRegisterAsync(user, dto.Password);
            
            if (!success)
            {
                return BadRequest(new { error = "Failed to create user" });
            }

            var response = new UserResponseDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Name = user.Name,
                DateRegistered = user.DateRegistered
            };

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, response);
        }

        /// <summary>
        /// Gets a user by ID (Admin only).
        /// </summary>
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            
            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            var response = new UserResponseDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Name = user.Name,
                DateRegistered = user.DateRegistered
            };

            return Ok(response);
        }

        /// <summary>
        /// Gets orders for a specific user.
        /// Admin can view any user's orders, users can only view their own.
        /// </summary>
        [HttpGet("{userId:guid}/orders")]
        [Authorize]
        [ProducesResponseType(typeof(IReadOnlyList<OrderResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserOrders(Guid userId)
        {
            var currentUserId = _currentUserService.UserId;
            var isAdmin = _currentUserService.IsAdmin;

            // Check authorization: must be admin or the owner
            if (!isAdmin && currentUserId != userId)
            {
                return Forbid();
            }

            var orders = await _orderService.GetByUserIdAsync(userId);
            return Ok(orders);
        }
    }
}
