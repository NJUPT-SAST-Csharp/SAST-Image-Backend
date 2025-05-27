using Mediator;
using SNS.Domain.Bookmarks.Events;

namespace SNS.Domain.Bookmarks;

internal sealed class CancelBookmarkCommandHandler(
    IBookmarkManager manager,
    IDomainEventContainer container
) : ICommandHandler<CancelBookmarkCommand>
{
    public async ValueTask<Unit> Handle(
        CancelBookmarkCommand request,
        CancellationToken cancellationToken
    )
    {
        var bookmark = await manager.GetBookmarkAsync(
            request.UserId,
            request.ImageId,
            cancellationToken
        );

        if (bookmark is null)
        {
            return Unit.Value;
        }

        await manager.CancelBookmarkAsync(bookmark, cancellationToken);

        container.AddDomainEvent(new BookmarkCancelledDomainEvent(request.UserId, request.ImageId));
        return Unit.Value;
    }
}
