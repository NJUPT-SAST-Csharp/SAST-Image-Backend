using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.Events;
using Domain.Core.Event;

namespace Application.AlbumServices.EventHandlers;

internal sealed class AlbumRestoredEventHandler(IAlbumModelRepository repository)
    : IDomainEventHandler<AlbumRestoredEvent>
{
    public async ValueTask Handle(AlbumRestoredEvent e, CancellationToken cancellationToken)
    {
        var album = await repository.GetAsync(e.Album, cancellationToken);

        album.Restore(e);
    }
}
