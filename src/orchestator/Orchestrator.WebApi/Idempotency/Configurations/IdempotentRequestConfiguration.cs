using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orchestrator.WebApi.Idempotency.Entities;

namespace Orchestrator.WebApi.Idempotency.Configurations;

internal sealed class IdempotentRequestConfiguration : IEntityTypeConfiguration<IdempotentRequest>
{
    public void Configure(EntityTypeBuilder<IdempotentRequest> builder)
    {
        builder.HasKey(idempotentRequest => idempotentRequest.Id);
        
        builder.Property(idempotentRequest => idempotentRequest.RequestId)
            .IsRequired();

        builder.Property(idempotentRequest => idempotentRequest.Name)
            .IsRequired()
            .HasMaxLength(255);
    }
}