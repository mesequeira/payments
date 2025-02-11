using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orchestrator.WebApi.Orders.Entities;

namespace Orchestrator.WebApi.Orders.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(order => order.Id);
        
        builder.Property(order => order.Id)
            .ValueGeneratedOnAdd();

        builder.Property(order => order.Id)
            .HasColumnName("OrderId");

        builder.Property(order => order.Amount)
            .IsRequired();

        builder.Property(order => order.TransactionId)
            .IsRequired();

        builder.Property(order => order.CreatedOnUtc)
            .IsRequired();
    }
}