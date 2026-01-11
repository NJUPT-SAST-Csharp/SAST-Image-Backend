using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.Events;
using Domain.Core.Event;

namespace Application.AlbumServices.EventHandlers;

internal sealed class AlbumTitleUpdatedEventHandler(IAlbumModelRepository repository)
    : IDomainEventHandler<AlbumTitleUpdatedEvent>
{
    public async ValueTask Handle(AlbumTitleUpdatedEvent e, CancellationToken cancellationToken)
    {
        var album = await repository.GetAsync(e.Album, cancellationToken);

        album.UpdateTitle(e);
    }
}
