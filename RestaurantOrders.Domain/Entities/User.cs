using System;
using RestaurantOrders.Domain.Enums;

namespace RestaurantOrders.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string UserName { get; set; } = null!;

        // Keep both in case you implement manual hashing; PasswordSalt can be null when using external identity providers
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }

        public UserRole Role { get; set; } = UserRole.User;
        public DateTime DateRegistered { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
