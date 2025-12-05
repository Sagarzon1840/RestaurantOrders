using RestaurantOrders.Domain.Entities;

namespace RestaurantOrders.Application.Interfaces
{
    /// <summary>
    /// Service for JWT token generation and validation.
    /// </summary>
    public interface IJwtService
    {
        string GenerateToken(User user);
        DateTime GetTokenExpiration();
    }
}
