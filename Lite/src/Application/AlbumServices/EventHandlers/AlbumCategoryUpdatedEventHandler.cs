using Domain.AlbumAggregate.Events;
using Domain.Core.Event;

namespace Application.AlbumServices.EventHandlers;

internal sealed class AlbumCategoryUpdatedEventHandler(IAlbumModelRepository repository)
    : IDomainEventHandler<AlbumCategoryUpdatedEvent>
{
    public async ValueTask Handle(AlbumCategoryUpdatedEvent e, CancellationToken cancellationToken)
    {
        var album = await repository.GetAsync(e.Album, cancellationToken);

        album.UpdateCategory(e);
    }
}
