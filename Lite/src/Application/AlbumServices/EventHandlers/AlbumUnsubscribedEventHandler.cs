using Domain.AlbumAggregate.Events;
using Domain.Core.Event;

namespace Application.AlbumServices.EventHandlers;

internal sealed class AlbumUnsubscribedEventHandler(ISubscribeModelRepository repository)
    : IDomainEventHandler<AlbumUnsubscribedEvent>
{
    public async ValueTask Handle(AlbumUnsubscribedEvent e, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(e.Album, e.User, cancellationToken);
    }
}
