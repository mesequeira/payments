using Cross.SharedKernel.Messages;
using Cross.SharedKernel.Results;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Orchestrator.WebApi.Idempotency.Messages;
using Payments.Events.Orders;

namespace Orchestrator.WebApi.Orders.Commands.Create;

public sealed record CreateOrderCommand(
    double Amount
) : IdempotentCommand, ITransactionalCommand;

public sealed class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(request => request.Amount)
            .GreaterThan(0)
            .WithErrorCode(OrderCodes.AmountGreaterThanZero);
    }
}

public sealed class CreateOrderCommandHandler(
    IBus publisher
) : IRequestHandler<CreateOrderCommand, Result>
{
    public async Task<Result> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderCreatedEvent = request.Adapt<OrderCreatedEvent>();
            
        await publisher.Publish(orderCreatedEvent, cancellationToken);

        return Result.Success();
    }
}

public sealed class OrdersModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("orders", CreateOrderAsync)
            .Produces<Unit>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName("CreateOrder")
            .WithTags("Orders");
    }

    private static async Task<IResult> CreateOrderAsync(
        [FromServices] ISender sender,
        [FromBody] CreateOrderCommand createOrderCommand,
        CancellationToken cancellationToken
    )
    {
        return await sender.Send(createOrderCommand, cancellationToken).HandleResultAsync();
    }
}