using Domain.AlbumAggregate.Events;
using Domain.Core.Event;

namespace Application.AlbumServices.EventHandlers;

internal sealed class AlbumRemovedEventHandler(IAlbumModelRepository repository)
    : IDomainEventHandler<AlbumRemovedEvent>
{
    public async ValueTask Handle(AlbumRemovedEvent e, CancellationToken cancellationToken)
    {
        var album = await repository.GetAsync(e.Album, cancellationToken);

        album.Remove(e);
    }
}
