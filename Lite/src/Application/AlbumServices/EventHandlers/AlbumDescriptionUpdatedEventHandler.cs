using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.Events;
using Domain.Core.Event;

namespace Application.AlbumServices.EventHandlers;

internal sealed class AlbumDescriptionUpdatedEventHandler(
    IAlbumModelRepository repository
) : IDomainEventHandler<AlbumDescriptionUpdatedEvent>
{
    public async ValueTask Handle(
        AlbumDescriptionUpdatedEvent e,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(e.Album, cancellationToken);

        album.UpdateDescription(e);
    }
}
