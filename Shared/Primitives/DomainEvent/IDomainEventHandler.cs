using MediatR;
using Shared.Primitives.DomainEvent;

namespace Primitives.DomainEvent
{
    public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent { }
}
