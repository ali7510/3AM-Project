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
    internal class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");
            builder.HasKey(oi => oi.Id);
            builder.Property(oi => oi.Id).ValueGeneratedOnAdd();
            builder.Property(oi => oi.Quantity).IsRequired();
            builder.Property(oi=>oi.Price_At_Purchase).HasPrecision(18,2).IsRequired();
            builder.HasOne(oi => oi.Order)
                        .WithMany(o => o.OrderItems)
                        .HasForeignKey(oi => oi.Order_Id)
                        .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oi => oi.Product)
                        .WithMany(p=>p.orderItems)
                        .HasForeignKey(oi => oi.Product_Id)
                        .OnDelete(DeleteBehavior.Restrict);
            builder.Ignore(oi => oi.Created_At);
        }
    }
}
