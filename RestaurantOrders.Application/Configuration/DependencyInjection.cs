using Microsoft.Extensions.DependencyInjection;
using RestaurantOrders.Application.UseCases;
using RestaurantOrders.Application.Interfaces;

namespace RestaurantOrders.Application.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
