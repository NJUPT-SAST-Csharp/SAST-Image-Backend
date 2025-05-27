using Exceptions.Exceptions;
using Mediator;
using Primitives;
using SastImg.Domain.AlbumAggregate;

namespace SastImg.Application.AlbumServices.RestoreAlbum;

public sealed class RestoreAlbumCommandHandler(IAlbumRepository repository, IUnitOfWork unitOfWork)
    : ICommandHandler<RestoreAlbumCommand>
{
    public async ValueTask<Unit> Handle(
        RestoreAlbumCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAlbumAsync(request.AlbumId, cancellationToken);
        if (request.Requester.IsAdmin || album.IsOwnedBy(request.Requester.Id))
        {
            album.Restore();
            await unitOfWork.CommitChangesAsync(cancellationToken);
        }
        else
        {
            throw new NoPermissionException();
        }

        return Unit.Value;
    }
}
