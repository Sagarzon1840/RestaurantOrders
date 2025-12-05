using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantOrders.Domain.Entities;

namespace RestaurantOrders.Infrastructure.Persistence.Configs
{
    public class MenuItemDBConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> b)
        {
            b.HasKey(m => m.Id);
            
            b.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            b.Property(m => m.BasePrice)
                .IsRequired()
                .HasPrecision(10, 2);
            
            b.Property(m => m.Category)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
            
            b.Property(m => m.SubCategory)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
            
            b.Property(m => m.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
            
            b.Property(m => m.CreatedAt)
                .IsRequired();

            b.HasIndex(m => m.Category);
            b.HasIndex(m => m.IsActive);
        }
    }
}
