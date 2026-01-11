using Domain.Core.Event;
using Domain.UserAggregate.Events;

namespace Application.UserServices.EventHandlers;

internal sealed class AvatarUpdatedEventHandler(IAvatarStorageManager manager)
    : IDomainEventHandler<AvatarUpdatedEvent>
{
    public async ValueTask Handle(AvatarUpdatedEvent e, CancellationToken cancellationToken)
    {
        await manager.UpdateAsync(e.User, e.Avatar.Stream, cancellationToken);
    }
}
