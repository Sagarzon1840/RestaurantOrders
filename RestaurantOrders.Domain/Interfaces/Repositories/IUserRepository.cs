using System;
using System.Threading.Tasks;
using RestaurantOrders.Domain.Entities;

namespace RestaurantOrders.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> FindByUserNameAsync(string username);
        Task<User> AddAsync(User user);
    }
}
