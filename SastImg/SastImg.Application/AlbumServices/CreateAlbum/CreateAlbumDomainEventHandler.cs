using Primitives.DomainEvent;
using SastImg.Domain.AlbumAggregate.Events;

namespace SastImg.Application.AlbumServices.CreateAlbum
{
    internal sealed class CreateAlbumDomainEventHandler
        : IDomainEventHandler<CreateAlbumDomainEvent>
    {
        public Task Handle(CreateAlbumDomainEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
