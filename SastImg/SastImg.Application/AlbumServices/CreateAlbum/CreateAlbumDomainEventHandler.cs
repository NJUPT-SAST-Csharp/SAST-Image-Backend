using Primitives.DomainEvent;
using SastImg.Domain.AlbumAggregate.AlbumEntity.Events;

namespace SastImg.Application.AlbumServices.CreateAlbum
{
    internal sealed class CreateAlbumDomainEventHandler
        : IDomainEventHandler<AlbumCreatedDomainEvent>
    {
        public Task Handle(
            AlbumCreatedDomainEvent notification,
            CancellationToken cancellationToken
        )
        {
            throw new NotImplementedException();
        }
    }
}
