using Orchestrator.WebApi.Abstractions.Contexts;
using Orchestrator.WebApi.Abstractions.Extensions;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

builder.Services.AddSwaggerGenConfiguration();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddMediatRConfiguration(assembly);
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