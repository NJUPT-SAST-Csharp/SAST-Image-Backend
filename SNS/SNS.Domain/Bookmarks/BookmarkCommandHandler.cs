using Primitives.Command;
using Shared.Primitives.DomainEvent;
using SNS.Domain.Bookmarks.Events;

namespace SNS.Domain.Bookmarks
{
    internal sealed class BookmarkCommandHandler(
        IBookmarkManager manager,
        IDomainEventContainer container
    ) : ICommandRequestHandler<BookmarkCommand>
    {
        private readonly IBookmarkManager _manager = manager;
        private readonly IDomainEventContainer _container = container;

        public async Task Handle(BookmarkCommand request, CancellationToken cancellationToken)
        {
            await _manager.BookmarkAsync(request.UserId, request.ImageId, cancellationToken);

            _container.AddDomainEvent(new BookmarkedDomainEvent(request.UserId, request.ImageId));
        }
    }
}
