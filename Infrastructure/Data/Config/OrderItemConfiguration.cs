using System;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // Like entity into 1 table
            builder.OwnsOne(o => o.ItemOrdered, a => a.WithOwner());

            // Config Price decimal(18,2)
            builder.Property(o => o.Price).HasColumnType("decimal(18, 2)");
        }
    }
}