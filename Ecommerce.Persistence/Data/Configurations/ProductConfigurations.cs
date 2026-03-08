using Ecommerce.Domain.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistence.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p=>p.Id);
            builder.Property(p=>p.Id).ValueGeneratedOnAdd();
            builder.Property(P=>P.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).HasMaxLength(200);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(p => p.Image_Url).HasColumnType("varchar(500)");
            builder.Property(p => p.Brand).HasColumnType("varchar(100)");
            builder.Property(p=>p.specsJson).HasColumnType("varchar(max)");
            builder.Property(p=>p.isActive).HasDefaultValue(true);
            builder.HasOne(p=>p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.Category_Id)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
