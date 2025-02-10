using System.Text.Json.Serialization;
using Cross.SharedKernel.Messages;
using Cross.SharedKernel.Results;

namespace Orchestrator.WebApi.Idempotency.Messages;

public abstract record IdempotentCommand : IRequest<Result>, IBaseTransactionCommand
{
    [JsonIgnore]
    public Guid RequestId { get; set; }
}