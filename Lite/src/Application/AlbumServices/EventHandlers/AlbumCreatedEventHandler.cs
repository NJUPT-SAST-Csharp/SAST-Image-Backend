using Domain.AlbumAggregate.Events;
using Domain.Core.Event;

namespace Application.AlbumServices.EventHandlers;

internal sealed class AlbumCreatedEventHandler(IAlbumModelRepository repository)
    : IDomainEventHandler<AlbumCreatedEvent>
{
    public async ValueTask Handle(AlbumCreatedEvent e, CancellationToken cancellationToken)
    {
        AlbumModel album = new(e);

        await repository.AddAsync(album, cancellationToken);
    }
}
