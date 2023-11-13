using MediatR;
using Shared.DomainPrimitives.DomainNotification;

namespace Common.Primitives.DomainNotification
{
    public interface IDomainNotificationHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : IDomainNotification { }
}
