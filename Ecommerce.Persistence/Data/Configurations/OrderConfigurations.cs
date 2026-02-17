using Ecommerce.Domain.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistence.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedOnAdd();
            builder.Property(o=>o.Total_Price).HasColumnType("decimal(18,2)");
            builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(50);
            builder.Property(o => o.Payment_Status).HasConversion<string>().HasMaxLength(50);
            builder.Property(o=>o.Created_At).HasDefaultValueSql("GETDATE()").HasColumnName("Created_At");

            builder.HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.User_Id)
                    .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(o => o.Order_Items)
                    .WithOne(oi => oi.Order)
                    .HasForeignKey(oi => oi.Order_Id)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Property(o => o.Created_At).HasColumnName("Added_At");

        }
    }
}
