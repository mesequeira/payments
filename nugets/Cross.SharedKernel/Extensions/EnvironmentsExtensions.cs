using Microsoft.Extensions.Hosting;

namespace Cross.SharedKernel.Extensions;

public static class EnvironmentsExtensions
{
    /// <summary>
    /// Specifies the Testing environment.
    /// </summary>
    /// <remarks>The Testing environment can enable or disable features that shouldn't be exposed in production.</remarks>
    private static readonly string Testing = "Testing";

    /// <summary>
    /// Specifies the Qa environment.
    /// </summary>
    /// <remarks>The Qa environment can enable or disable features that shouldn't be exposed in production.</remarks>
    private static readonly string Qa = "Qa";

    /// <summary>
    /// Checks if the current host environment name is <see cref="Testing"/>.
    /// </summary>
    /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
    /// <returns>True if the environment name is <see cref="Testing"/>, otherwise false.</returns>
    public static bool IsTesting(this IHostEnvironment hostEnvironment)
    {
        ArgumentNullException.ThrowIfNull(hostEnvironment);
        return hostEnvironment.IsEnvironment(Testing);
    }

    /// <summary>
    /// Checks if the current host environment name is <see cref="Qa"/>.
    /// </summary>
    /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
    /// <returns>True if the environment name is <see cref="Qa"/>, otherwise false.</returns>
    public static bool IsQa(this IHostEnvironment hostEnvironment)
    {
        ArgumentNullException.ThrowIfNull(hostEnvironment);
        return hostEnvironment.IsEnvironment(Qa);
    }
}
