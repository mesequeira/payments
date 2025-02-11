using MassTransit;

namespace Orchestrator.WebApi.Orders.Saga;

public class OrderState : SagaStateMachineInstance
{
    public long OrderStateId { get; set; }
    
    public Guid CorrelationId { get; set; } 
    public Guid TransactionId { get; set; }
    public string CurrentState { get; set; }  = default!;
    
    public DateTime CreatedOnUtc { get; set; }
}
