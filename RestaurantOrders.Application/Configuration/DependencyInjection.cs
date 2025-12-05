using Microsoft.Extensions.DependencyInjection;
using RestaurantOrders.Application.UseCases;
using RestaurantOrders.Application.Interfaces;
using RestaurantOrders.Domain.Interfaces.Services;
using RestaurantOrders.Domain.Services;

namespace RestaurantOrders.Application.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Domain services
            services.AddScoped<IDiscountPolicy, SimpleComboDiscountPolicy>();

            // Application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<AuthService>();

            return services;
        }
    }
}
