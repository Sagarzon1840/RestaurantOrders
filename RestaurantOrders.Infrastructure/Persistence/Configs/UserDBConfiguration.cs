using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantOrders.Domain.Entities;

namespace RestaurantOrders.Infrastructure.Persistence.Configs
{
    public class UserDBConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> b)
        {
            b.HasKey(u => u.Id);
            b.Property(u => u.Name).IsRequired().HasMaxLength(100);
            b.Property(u => u.UserName).IsRequired().HasMaxLength(50);
            b.Property(u => u.DateRegistered).IsRequired();
            b.Property(u => u.IsActive).HasDefaultValue(true);
            
            b.Property(u => u.Role)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            b.HasIndex(u => u.UserName).IsUnique();
        }
    }
}
