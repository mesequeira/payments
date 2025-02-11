namespace WebApi.SharedKernel.Options;

public sealed class MessageBrokerOptions
{
    public required string ConnectionString { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}