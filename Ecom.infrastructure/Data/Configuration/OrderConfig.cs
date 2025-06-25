using Ecom.Core.Entites.order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Data.Configuration
{
    public class OrderConfig : IEntityTypeConfiguration<orders>
    {
        public void Configure(EntityTypeBuilder<orders> builder)
        {
            builder.OwnsOne(x => x.shippingAddress, n => { n.WithOwner(); });
            builder.HasMany(x => x.OrderItems)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);
            builder.Property(x=>x.status)
                   .HasConversion(
                       x => x.ToString(),
                       x => (Status)Enum.Parse(typeof(Status), x))
                   .IsRequired();
            builder.Property(x => x.BuyerEmail).IsRequired();
            builder.Property(x=>x.SupTotal).IsRequired().HasColumnType("decimal(18,2)");

        }
    }
}
