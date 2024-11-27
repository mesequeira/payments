using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.SharedKernel.Extensions;

public static class MigrationsExtensions
{
    /// <summary>
    /// The extension method to apply the migrations to the database when the current environment is Development.
    /// If you are not in development, you should apply the migrations manually using the command line.
    /// </summary>
    /// <param name="app">The application to apply the migrations.</param>
    /// <returns>The application with the migrations applied.</returns>
    public static void UseMigrations<TContext>(this WebApplication app)
        where TContext : DbContext
    {
        using var scope = app.Services.CreateScope();

        using var context = scope.ServiceProvider.GetRequiredService<TContext>();

        context.Database.Migrate();
    }
}
