using System.Diagnostics;
using Cross.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApi.SharedKernel.Extensions;

public static class MigrationsExtensions
{
    /// <summary>
    /// The extension method that try to apply the migrations to the database when the current environment is Development.
    /// If you are not in development, you should apply the migrations manually using the command line.
    /// </summary>
    /// <param name="app">The application to apply the migrations.</param>
    /// <returns>The application with the migrations applied.</returns>
    public static void TryUseMigrations<TContext>(this WebApplication app)
        where TContext : DbContext
    {
        try
        {
            if (!app.Environment.IsDevelopment() || !app.Environment.IsTesting())
            {
                Trace.TraceWarning("Don't be a stupid and apply the migrations manually in production or you will lose your data.");
            }
            
            using var scope = app.Services.CreateScope();

            using var context = scope.ServiceProvider.GetRequiredService<TContext>();

            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            Trace.TraceError(ex.Message);
        }
        
    }
}
