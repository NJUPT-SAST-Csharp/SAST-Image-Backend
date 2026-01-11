using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.Events;
using Domain.Core.Event;

namespace Application.AlbumServices.EventHandlers;

internal sealed class AlbumTagsUpdatedEventHandler(IAlbumModelRepository repository)
    : IDomainEventHandler<AlbumTagsUpdatedEvent>
{
    public async ValueTask Handle(AlbumTagsUpdatedEvent e, CancellationToken cancellationToken)
    {
        var album = await repository.GetAsync(e.Id, cancellationToken);
        album.UpdateTags(e);
    }
}
