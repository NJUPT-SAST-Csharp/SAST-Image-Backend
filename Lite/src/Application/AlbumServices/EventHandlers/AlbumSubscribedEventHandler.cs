using Domain.AlbumAggregate.Events;
using Domain.Core.Event;

namespace Application.AlbumServices.EventHandlers;

internal sealed class AlbumSubscribedEventHandler(ISubscribeModelRepository repository)
    : IDomainEventHandler<AlbumSubscribedEvent>
{
    public async ValueTask Handle(AlbumSubscribedEvent e, CancellationToken cancellationToken)
    {
        SubscribeModel subscribe = new(e.Album.Value, e.User.Value);

        await repository.AddAsync(subscribe, cancellationToken);
    }
}
