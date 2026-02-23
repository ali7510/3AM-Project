using Ecommerce.Domain.CartModule;
using Ecommerce.Domain.UserModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistence.Data.Configurations
{
    public class CartConfigurations : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts");
            builder.HasKey(c => c.Id);
            builder.HasOne(c => c.User)
                    .WithOne(u => u.Cart)
                    .HasForeignKey<Cart>(c => c.User_Id)
                    .OnDelete(DeleteBehavior.Cascade);
            builder.Property(c => c.Created_At).HasColumnName("Created_At").HasDefaultValueSql("GETDATE()");
        }
    }
}
