namespace Cross.SharedKernel.Abstractions;

/// <summary>
/// Static class to store and dispatch domain events.
/// </summary>
public static class DomainEvents
{
    /// <summary>
    /// A list of domain events that will be dispatched when the entity is saved.
    /// </summary>
    private static List<IDomainEvent> _domainEvents = [];

    /// <summary>
    /// A collection of domain events that will be dispatched when the entity is saved. It is a read-only collection to avoid modifications from outside the class.
    /// </summary>
    /// <returns></returns>
    public static IReadOnlyCollection<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }

    /// <summary>
    /// Once the domain events are dispatched, we need to clear the list of domain events.
    /// </summary>
    public static void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// This method is called when we want to store a domain event that will be
    /// dispatched when the entity is saved to handle it as a side effect.
    /// </summary>
    /// <param name="domainEvent">The event to store</param>
    public static void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}