namespace Domain.Event;

public interface IDomainEventContainer
{
    public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    public void ClearDomainEvents();

    public void AddDomainEvent(IDomainEvent domainEvent);
}
