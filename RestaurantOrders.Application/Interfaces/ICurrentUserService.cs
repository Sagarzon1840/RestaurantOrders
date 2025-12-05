namespace RestaurantOrders.Application.Interfaces
{
    /// <summary>
    /// Provides information about the currently authenticated user.
    /// </summary>
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? Username { get; }
        string? Role { get; }
        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
    }
}
