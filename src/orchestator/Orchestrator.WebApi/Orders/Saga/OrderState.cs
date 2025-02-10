using MassTransit;

namespace Orchestrator.WebApi.Orders.Saga;

public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; } 
    public string CurrentState { get; set; }  = default!;
    public Guid IdempotencyKey { get; set; }
    public decimal Amount { get; set; }
    public string OrderId { get; set; } = default!;
    public bool PaymentSuccessful { get; set; }
    
    public DateTime CreatedOnUtc { get; set; }
}
