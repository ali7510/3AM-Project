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
    public class ShipmentConfigurations : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.ToTable("Shipments");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).ValueGeneratedOnAdd();
            builder.Property(s => s.TrackingNumber).HasMaxLength(500);
            builder.Property(s => s.Company).HasMaxLength(100);
            builder.Property(s=>s.Created_At).HasDefaultValueSql("GETDATE()").HasColumnName("Shipped_At");
            builder.Property(s => s.Status).HasConversion<string>().HasMaxLength(50);
            builder.HasOne(s => s.Order)
                    .WithOne(o => o.Shipment)
                    .HasForeignKey<Shipment>(s => s.Order_Id)
                    .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
