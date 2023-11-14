using MediatR;

namespace Shared.Primitives.DomainNotification
{
    public interface IDomainNotificationHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : IDomainNotification { }
}
