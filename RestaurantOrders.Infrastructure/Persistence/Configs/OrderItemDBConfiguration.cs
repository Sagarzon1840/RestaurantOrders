using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantOrders.Domain.Entities;

namespace RestaurantOrders.Infrastructure.Persistence.Configs
{
    public class OrderItemDBConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> b)
        {
            b.HasKey(oi => oi.Id);
            
            b.Property(oi => oi.OrderId)
                .IsRequired();
            
            b.Property(oi => oi.ItemId)
                .IsRequired();
            
            b.Property(oi => oi.ItemNameSnapshot)
                .IsRequired()
                .HasMaxLength(100);
            
            b.Property(oi => oi.UnitPriceSnapshot)
                .IsRequired()
                .HasPrecision(10, 2);
            
            b.Property(oi => oi.CategorySnapshot)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
            
            b.Property(oi => oi.SubCategorySnapshot)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
            
            b.Property(oi => oi.Quantity)
                .IsRequired()
                .HasDefaultValue(1);

            // Relationship with MenuItem
            b.HasOne(oi => oi.MenuItem)
                .WithMany()
                .HasForeignKey(oi => oi.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(oi => oi.OrderId);
            b.HasIndex(oi => oi.ItemId);
        }
    }
}
