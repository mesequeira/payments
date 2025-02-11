using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orchestrator.WebApi.Orders.Saga;

namespace Orchestrator.WebApi.Orders.Configurations;

public class OrderStateConfiguration : IEntityTypeConfiguration<OrderState>
{
    public void Configure(EntityTypeBuilder<OrderState> builder)
    {
        builder.HasKey(orderState => orderState.OrderStateId);

        builder.Property(orderState => orderState.OrderStateId)
            .ValueGeneratedOnAdd();

        builder.Property(orderState => orderState.CorrelationId)
            .HasMaxLength(100);
        
        builder.Property(orderState => orderState.CurrentState)
            .HasMaxLength(255);

        builder.Property(orderState => orderState.TransactionId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(orderState => orderState.CreatedOnUtc)
            .IsRequired();
    }
}