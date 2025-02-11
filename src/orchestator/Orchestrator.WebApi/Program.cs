using Orchestrator.WebApi.Abstractions.Contexts;
using Orchestrator.WebApi.Abstractions.Extensions;
using Orchestrator.WebApi.Idempotency.Behaviors;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

builder.Services.AddSwaggerGenConfiguration(assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddMediatRConfiguration(assembly, configuration =>
{
    configuration.AddOpenBehavior(typeof(IdempotentPipelineBehavior<,>));
});
builder.Services.AddPersistenceConfiguration(builder.Configuration);
builder.Services.AddMessageBrokerConfiguration(builder.Configuration);
builder.Services.AddCarter();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.TryUseMigrations<ApplicationDbContext>();
}

app.UseHttpsRedirection();

app.MapCarter();
await app.RunAsync();