namespace Cross.SharedKernel.Abstractions;

public class Entity : AggregateRoot
{
    /// <summary>
    /// The main identifier of the entity.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The date and time when the entity was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; set; }

    /// <summary>
    /// The date and time when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
