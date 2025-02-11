using System.Net;
using Cross.SharedKernel.Interfaces;
using Cross.SharedKernel.Results;
using Orchestrator.WebApi.Idempotency.Entities;
using Orchestrator.WebApi.Idempotency.Messages;
using Orchestrator.WebApi.Idempotency.Repositories;

namespace Orchestrator.WebApi.Idempotency.Behaviors;

public sealed class IdempotentPipelineBehavior<TRequest, TResponse>(
    ILogger<IdempotentPipelineBehavior<TRequest, TResponse>> logger,
    IHttpContextAccessor accessor,
    IIdempotentRequestRepository idempotentRequestRepository,
    IUnitOfWork unitOfWork
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IdempotentCommand
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var context = accessor.HttpContext;

            if (context is null)
            {
                return (TResponse)(object)Result.Failure(
                    IdempotentRequestErrors.InvalidHttpContext,
                    HttpStatusCode.InternalServerError
                );
            }

            context.Request.Headers.TryGetValue("X-Idempotency-Key", out var requestId);

            if (string.IsNullOrWhiteSpace(requestId))
            {
                logger.LogInformation("The idempotency key header is missing.");
                
                return (TResponse)(object)Result.Failure(
                    IdempotentRequestErrors.MissingIdempotentKeyHeader,
                    HttpStatusCode.NotFound
                );
            }
            
            logger.LogInformation($"We receive a request with the idempotency key {requestId}.");

            if (!Guid.TryParse(requestId, out Guid parsedRequestId))
            {
                logger.LogInformation($"The idempotency key {requestId} has an invalid format.");
                
                return (TResponse)(object)Result.Failure(
                    IdempotentRequestErrors.InvalidFormatIdempotentKeyHeader,
                    HttpStatusCode.BadRequest
                );
                
            }

            if (await idempotentRequestRepository.ExistsAsync(parsedRequestId, cancellationToken))
            {
                logger.LogInformation($"The idempotency key {requestId} has already been processed.");
                return (TResponse)(object)Result.Created();
            }
            
            logger.LogInformation($"The idempotency key {requestId} has not been processed yet. We proceed to create it.");

            await idempotentRequestRepository.CreateAsync(
                parsedRequestId,
                typeof(TRequest).Name,
                cancellationToken
            );
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            request.TransactionId = parsedRequestId;

            return await next();
        }
        catch (Exception)
        {
            return (TResponse)(object)Result.Failure(
                IdempotentRequestErrors.SomethingWentWrongObtainingIdempoencyKey,
                HttpStatusCode.InternalServerError
            );
        }
    }
}