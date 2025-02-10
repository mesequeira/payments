using Cross.SharedKernel.Results;
using Microsoft.Extensions.Logging;

namespace Cross.SharedKernel.Behaviors;

/// <summary>
/// This class is used to intercept all the requests that implement the <see cref="Result"/> as the response type and log the processing of the request.
/// </summary>
/// <param name="logger">The logger to log the processing of the request</param>
/// <typeparam name="TRequest">The incoming request type</typeparam>
/// <typeparam name="TResponse">The response type</typeparam>
public sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var requestName = typeof(TRequest).Name;

        logger.LogInformation("Processing request {RequestName}", requestName);

        var result = await next();

        if (result.IsSuccess)
        {
            logger.LogInformation("Completed request {RequestName}", requestName);
        }
        else
        {
            logger.LogError("Completed request {RequestName} with error", requestName);
        }

        return result;
    }
}
