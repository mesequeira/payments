using Cross.SharedKernel.Errors;

namespace Orchestrator.WebApi.Idempotency.Entities;

public static class IdempotentRequestErrors
{
    public static Error MissingIdempotentKeyHeader => new(
        nameof(MissingIdempotentKeyHeader),
        "We don't found the header 'X-Idempotency-Key' in the request. Please, provide it with a valid Guid value.",
        ErrorType.NotFound
    );
    
    public static Error InvalidHttpContext => new(
        nameof(InvalidHttpContext),
        "While we were trying to obtain the request id from the header, the HttpContext was null.",
        ErrorType.Validation
    );
    
    public static Error InvalidFormatIdempotentKeyHeader => new(
        nameof(InvalidFormatIdempotentKeyHeader),
        "The value of the header 'X-Idempotency-Key' is not a valid Guid.",
        ErrorType.Validation
    );
    
    public static Error SomethingWentWrongObtainingIdempoencyKey => new(
        nameof(SomethingWentWrongObtainingIdempoencyKey),
        "While we were trying to obtain the idempotency key from the headers of the request, something went wrong.",
        ErrorType.Conflict
    );
}