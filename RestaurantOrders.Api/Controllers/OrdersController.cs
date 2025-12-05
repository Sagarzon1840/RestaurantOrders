using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOrders.Application.DTOs;
using RestaurantOrders.Application.Interfaces;

namespace RestaurantOrders.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICurrentUserService _currentUserService;

        public OrdersController(IOrderService orderService, ICurrentUserService currentUserService)
        {
            _orderService = orderService;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Creates a new order for the authenticated user.
        /// Returns the order with calculated totals and discounts.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            var userId = _currentUserService.UserId;
            if (!userId.HasValue)
            {
                return Unauthorized(new { error = "User not authenticated" });
            }

            var order = await _orderService.CreateOrderAsync(userId.Value, dto);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        /// <summary>
        /// Gets all orders. Admins see all orders, users see only their own.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<OrderResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            var userId = _currentUserService.UserId;
            var isAdmin = _currentUserService.IsAdmin;

            var orders = await _orderService.GetAllAsync(userId, isAdmin);
            return Ok(orders);
        }

        /// <summary>
        /// Gets a specific order by ID.
        /// Admins can see any order, users can only see their own.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = _currentUserService.UserId;
            if (!userId.HasValue)
            {
                return Unauthorized(new { error = "User not authenticated" });
            }

            var order = await _orderService.GetByIdAsync(id, userId.Value, _currentUserService.IsAdmin);
            
            if (order == null)
            {
                return NotFound(new { error = "Order not found" });
            }

            return Ok(order);
        }

        /// <summary>
        /// Updates an existing order.
        /// Admins can update any order, users can only update their own.
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderDto dto)
        {
            var userId = _currentUserService.UserId;
            if (!userId.HasValue)
            {
                return Unauthorized(new { error = "User not authenticated" });
            }

            var updated = await _orderService.UpdateOrderAsync(id, dto, userId.Value, _currentUserService.IsAdmin);
            
            if (updated == null)
            {
                return NotFound(new { error = "Order not found" });
            }

            return Ok(updated);
        }

        /// <summary>
        /// Deletes an order.
        /// Admins can delete any order, users can only delete their own.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = _currentUserService.UserId;
            if (!userId.HasValue)
            {
                return Unauthorized(new { error = "User not authenticated" });
            }

            var result = await _orderService.DeleteOrderAsync(id, userId.Value, _currentUserService.IsAdmin);
            
            if (!result)
            {
                return NotFound(new { error = "Order not found" });
            }

            return NoContent();
        }
    }
}
