using Primitives.Command;
using Shared.Primitives.DomainEvent;
using SNS.Domain.Bookmarks.Events;

namespace SNS.Domain.Bookmarks
{
    internal sealed class CancelBookmarkCommandHandler(
        IBookmarkManager manager,
        IDomainEventContainer container
    ) : ICommandRequestHandler<CancelBookmarkCommand>
    {
        private readonly IBookmarkManager _manager = manager;
        private readonly IDomainEventContainer _container = container;

        public async Task Handle(CancelBookmarkCommand request, CancellationToken cancellationToken)
        {
            var bookmark = await _manager.GetBookmarkAsync(
                request.UserId,
                request.ImageId,
                cancellationToken
            );

            if (bookmark is null)
            {
                return;
            }

            await _manager.CancelBookmarkAsync(bookmark, cancellationToken);

            _container.AddDomainEvent(
                new BookmarkCancelledDomainEvent(request.UserId, request.ImageId)
            );
        }
    }
}
