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
    public class DeliveryMethodConfig : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            builder.HasData(new DeliveryMethod { Id = 1, DeliveryTime = "only a week", Description = "the fast Delivery in the world", Name = "DHL", Price = 15 },
                            new DeliveryMethod { Id = 2, DeliveryTime = "only a two week", Description = "Make your Product save", Name = "XXX", Price = 12 });
        }
    }
}
