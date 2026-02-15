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
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Ignore(c => c.Created_At);
            builder.HasOne(c => c.Parent_Category)
                    .WithMany(pc => pc.Sub_Categories)
                    .HasForeignKey(c => c.Parent_Category_Id)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
