using Carter;
using FluentValidation;
using Inventory.WebApi.Abstractions.Contexts;
using Inventory.WebApi.Abstractions.Extensions;
using Orchestrator.WebApi.Abstractions.Extensions;
using WebApi.SharedKernel.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var assembly = typeof(Program).Assembly;

builder.Services.AddSwaggerGenConfiguration(assembly);
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