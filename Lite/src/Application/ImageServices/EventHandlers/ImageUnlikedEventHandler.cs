using Domain.AlbumAggregate.Events;
using Domain.Core.Event;

namespace Application.ImageServices.EventHandlers;

internal sealed class ImageUnlikedEventHandler(ILikeModelRepository repository)
    : IDomainEventHandler<ImageUnlikedEvent>
{
    public async ValueTask Handle(ImageUnlikedEvent e, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(e.Image, e.User, cancellationToken);
    }
}
