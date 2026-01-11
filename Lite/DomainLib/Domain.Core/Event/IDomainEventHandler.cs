using Domain.Event;
using Mediator;

namespace Domain.Core.Event;

public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent { }
