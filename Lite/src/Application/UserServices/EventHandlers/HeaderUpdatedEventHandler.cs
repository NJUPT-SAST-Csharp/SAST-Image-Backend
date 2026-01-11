using Domain.Core.Event;
using Domain.UserAggregate.Events;

namespace Application.UserServices.EventHandlers;

internal sealed class HeaderUpdatedEventHandler(IHeaderStorageManager manager)
    : IDomainEventHandler<HeaderUpdatedEvent>
{
    public async ValueTask Handle(HeaderUpdatedEvent e, CancellationToken cancellationToken)
    {
        await manager.UpdateAsync(e.User, e.Header.Stream, cancellationToken);
    }
}
