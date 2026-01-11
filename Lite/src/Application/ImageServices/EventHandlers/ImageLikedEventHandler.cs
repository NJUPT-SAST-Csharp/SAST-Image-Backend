using Domain.AlbumAggregate.Events;
using Domain.Core.Event;

namespace Application.ImageServices.EventHandlers;

internal sealed class ImageLikedEventHandler(ILikeModelRepository repository)
    : IDomainEventHandler<ImageLikedEvent>
{
    public async ValueTask Handle(ImageLikedEvent e, CancellationToken cancellationToken)
    {
        LikeModel like = new(e.Image.Value, e.User.Value);

        await repository.AddAsync(like, cancellationToken);
    }
}
