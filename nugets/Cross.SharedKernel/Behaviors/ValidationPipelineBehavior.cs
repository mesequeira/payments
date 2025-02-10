using System.Net;
using Cross.SharedKernel.Errors;
using Cross.SharedKernel.Exceptions;
using Cross.SharedKernel.Results;
using FluentValidation;
using FluentValidation.Results;

namespace Cross.SharedKernel.Behaviors;

/// <summary>
/// A pipeline behavior that validates the request using the provided validators. If detected any validation failures, it returns a <see cref="Result"/> with the validation errors attached to a <see cref="ValidationError"/>.
/// </summary>
/// <param name="validators">The validators to validate the request.</param>
/// <typeparam name="TRequest">The type of the request to validate.</typeparam>
/// <typeparam name="TResponse">The type of the response to return.</typeparam>
public sealed class ValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    /// <summary>
    /// Using reflection to invoke the appropriate method to create a validation failure result.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <param name="next">The next handler in the pipeline.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The response if the request is valid, otherwise a <see cref="Result"/> with the validation errors.</returns>
    /// <exception cref="ValidationException">Thrown when the request is invalid.</exception>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        // Catch the list of validation failures
        var validationFailures = await ValidateAsync(request);

        if (validationFailures.Length == 0)
        {
            return await next();
        }

        // If the response is a generic type and is a Result, create a validation failure result
        if (
            typeof(TResponse).IsGenericType
            && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>)
        )
        {
            var resultType = typeof(TResponse).GetGenericArguments()[0];

            var failureMethod = typeof(Result<>)
                .MakeGenericType(resultType)
                .GetMethod(nameof(Result<object>.ValidationFailure));

            if (failureMethod is not null)
            {
                // Invoke the ValidationFailure method to create a validation failure result
                return (TResponse)
                    failureMethod.Invoke(null, [CreateValidationError(validationFailures)])!;
            }
        }
        else if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)
                (object)
                    Result.Failure(
                        CreateValidationError(validationFailures),
                        HttpStatusCode.Conflict
                    );
        }

        throw new ValidationException(validationFailures);
    }

    /// <summary>
    /// Loops through the validators and validates the request looking for possible validation failures.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>An array of possible validation failures.</returns>
    private async Task<ValidationFailure[]> ValidateAsync(TRequest request)
    {
        if (!validators.Any())
        {
            return [];
        }

        var context = new ValidationContext<TRequest>(request);

        // Dispatch the validation tasks and wait for all of them to complete
        var validationResults = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(context))
        );

        // Loop through the validation results and get the validation failures
        return validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .ToArray();
    }

    /// <summary>
    /// Creates a validation error that inherits from the <see cref="Error"/> class containing the validation failures inside <see cref="ValidationError.Errors"/>
    /// </summary>
    /// <param name="validationFailures">The validation failures to create the <see cref="ValidationError"/> from.</param>
    /// <returns>A <see cref="ValidationError"/> containing the validation failures.</returns>
    private static ValidationError CreateValidationError(
        IEnumerable<ValidationFailure> validationFailures
    ) => new(validationFailures.Select(f => Error.Problem(f.ErrorCode, f.ErrorMessage)).ToArray());
}
