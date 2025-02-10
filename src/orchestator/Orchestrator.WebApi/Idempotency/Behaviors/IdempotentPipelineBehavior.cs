using System.Net;
using Cross.SharedKernel.Interfaces;
using Cross.SharedKernel.Results;
using Orchestrator.WebApi.Idempotency.Entities;
using Orchestrator.WebApi.Idempotency.Messages;
using Orchestrator.WebApi.Idempotency.Repositories;

namespace Orchestrator.WebApi.Idempotency.Behaviors;

public sealed class IdempotentPipelineBehavior<TRequest, TResponse>(
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
                return (TResponse)(object)Result.Failure(
                    IdempotentRequestErrors.MissingIdempotentKeyHeader,
                    HttpStatusCode.NotFound
                );
            }

            if (!Guid.TryParse(requestId, out Guid parsedRequestId))
            {
                return (TResponse)(object)Result.Failure(
                    IdempotentRequestErrors.InvalidFormatIdempotentKeyHeader,
                    HttpStatusCode.BadRequest
                );
            }

            if (await idempotentRequestRepository.ExistsAsync(parsedRequestId, cancellationToken))
            {
                return (TResponse)(object)Result.Created();
            }

            await idempotentRequestRepository.CreateAsync(
                parsedRequestId,
                typeof(TRequest).Name,
                cancellationToken
            );
            
            await unitOfWork.SaveChangesAsync(cancellationToken);

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