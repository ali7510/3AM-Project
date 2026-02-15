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
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(255);
            builder.Property(u=>u.Phone).HasColumnType("varchar").HasMaxLength(13);
            builder.Property(u => u.isActive).HasDefaultValue(true);
            builder.Property(u => u.Role).HasConversion<string>().HasMaxLength(20);
            builder.Property(u=>u.Created_At).HasColumnName("Joined_At").HasDefaultValueSql("GETDATE()");
        }
    }
}
