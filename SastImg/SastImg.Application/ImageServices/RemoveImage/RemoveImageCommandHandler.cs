using Exceptions.Exceptions;
using Mediator;
using Primitives;
using SastImg.Domain.AlbumAggregate;

namespace SastImg.Application.ImageServices.RemoveImage;

public sealed class RemoveImageCommandHandler(IAlbumRepository repository, IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveImageCommand>
{
    public async ValueTask<Unit> Handle(
        RemoveImageCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAlbumAsync(request.AlbumId, cancellationToken);

        if (request.Requester.IsAdmin || album.IsManagedBy(request.Requester.Id))
        {
            album.RemoveImage(request.ImageId);
            await unitOfWork.CommitChangesAsync(cancellationToken);
        }
        else
        {
            throw new NoPermissionException();
        }

        return Unit.Value;
    }
}
