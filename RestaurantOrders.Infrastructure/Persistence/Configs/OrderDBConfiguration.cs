using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantOrders.Domain.Entities;

namespace RestaurantOrders.Infrastructure.Persistence.Configs
{
    public class OrderDBConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> b)
        {
            b.HasKey(o => o.Id);
            
            b.Property(o => o.UserId)
                .IsRequired();
            
            b.Property(o => o.CreatedAt)
                .IsRequired();
            
            b.Property(o => o.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
            
            b.Property(o => o.Subtotal)
                .IsRequired()
                .HasPrecision(10, 2);
            
            b.Property(o => o.DiscountApplied)
                .IsRequired()
                .HasPrecision(10, 2);
            
            b.Property(o => o.Total)
                .IsRequired()
                .HasPrecision(10, 2);

            // Relationship with User
            b.HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship with OrderItems
            b.HasMany(o => o.Items)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ignore the private backing field for Items
            b.Metadata.FindNavigation(nameof(Order.Items))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            b.HasIndex(o => o.UserId);
            b.HasIndex(o => o.Status);
            b.HasIndex(o => o.CreatedAt);
        }
    }
}
