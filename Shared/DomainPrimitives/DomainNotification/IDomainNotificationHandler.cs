using MediatR;

namespace Shared.DomainPrimitives.DomainNotification
{
    public interface IDomainNotificationHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : IDomainNotification { }
}
