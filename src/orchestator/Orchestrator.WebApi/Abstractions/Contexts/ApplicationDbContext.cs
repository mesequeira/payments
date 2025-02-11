using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Orchestrator.WebApi.Idempotency.Entities;
using Orchestrator.WebApi.Orders.Entities;
using Orchestrator.WebApi.Orders.Saga;

namespace Orchestrator.WebApi.Abstractions.Contexts;

public sealed class ApplicationDbContext(DbContextOptions options) 
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS));
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }
    
    public DbSet<IdempotentRequest> IdempotentRequests { get; set; }
    public DbSet<OrderState> OrderStates { get; set; }
    public DbSet<Order> Orders { get; set; }
    
}