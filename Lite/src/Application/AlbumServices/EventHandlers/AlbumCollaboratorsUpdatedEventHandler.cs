using Domain.AlbumAggregate.Events;
using Domain.Core.Event;

namespace Application.AlbumServices.EventHandlers;

internal sealed class AlbumCollaboratorsUpdatedEventHandler(IAlbumModelRepository repository)
    : IDomainEventHandler<AlbumCollaboratorsUpdatedEvent>
{
    public async ValueTask Handle(
        AlbumCollaboratorsUpdatedEvent e,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(e.Album, cancellationToken);

        album.UpdateCollaborators(e);
    }
}
