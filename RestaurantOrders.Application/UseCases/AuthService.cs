using RestaurantOrders.Application.DTOs;
using RestaurantOrders.Application.Interfaces;
using RestaurantOrders.Domain.Entities;

namespace RestaurantOrders.Application.UseCases
{
    public class AuthService
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthService(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await _userService.UserLoginAsync(request.Username, request.Password);
            if (user == null) return null;

            var token = _jwtService.GenerateToken(user);
            var expiration = _jwtService.GetTokenExpiration();

            return new LoginResponseDto
            {
                Token = token,
                ExpiresAt = expiration,
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Name = user.Name,
                    Role = user.Role.ToString()
                }
            };
        }
    }
}
