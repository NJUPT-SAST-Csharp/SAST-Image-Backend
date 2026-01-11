using Domain.AlbumAggregate.Events;
using Domain.Core.Event;

namespace Application.AlbumServices.EventHandlers;

internal sealed class AlbumAccessLevelUpdatedEventHandler(IAlbumModelRepository repository)
    : IDomainEventHandler<AlbumAccessLevelUpdatedEvent>
{
    public async ValueTask Handle(
        AlbumAccessLevelUpdatedEvent e,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(e.Album, cancellationToken);

        album.UpdateAccessLevel(e);
    }
}
