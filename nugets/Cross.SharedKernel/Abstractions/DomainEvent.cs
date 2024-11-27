using MediatR;

namespace Cross.SharedKernel.Abstractions;

/// <summary>
/// The interface that all domain events should inherit from.
/// </summary>
public interface IDomainEvent : INotification;
