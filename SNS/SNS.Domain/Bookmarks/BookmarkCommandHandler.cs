using Mediator;
using SNS.Domain.Bookmarks.Events;

namespace SNS.Domain.Bookmarks;

internal sealed class BookmarkCommandHandler(
    IBookmarkManager manager,
    IDomainEventContainer container
) : ICommandHandler<BookmarkCommand>
{
    public async ValueTask<Unit> Handle(
        BookmarkCommand request,
        CancellationToken cancellationToken
    )
    {
        await manager.BookmarkAsync(request.UserId, request.ImageId, cancellationToken);

        container.AddDomainEvent(new BookmarkedDomainEvent(request.UserId, request.ImageId));

        return Unit.Value;
    }
}
