using Ecommerce.Domain.CartModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistence.Data.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");
            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Quantity).IsRequired();
            builder.HasOne(ci => ci.Cart)
                    .WithMany(c => c.CartItems)
                    .HasForeignKey(ci => ci.Cart_Id)
                    .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ci => ci.Product)
                    .WithMany(p => p.cartItems)
                    .HasForeignKey(ci => ci.Product_Id)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
