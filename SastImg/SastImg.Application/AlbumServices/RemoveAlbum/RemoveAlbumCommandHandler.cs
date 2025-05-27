using Exceptions.Exceptions;
using Mediator;
using Primitives;
using SastImg.Domain.AlbumAggregate;

namespace SastImg.Application.AlbumServices.RemoveAlbum;

public sealed class RemoveAlbumCommandHandler(IAlbumRepository repository, IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveAlbumCommand>
{
    private readonly IAlbumRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async ValueTask<Unit> Handle(
        RemoveAlbumCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await _repository.GetAlbumAsync(request.AlbumId, cancellationToken);

        if (request.Requester.IsAdmin || album.IsOwnedBy(request.Requester.Id))
        {
            album.Remove();
            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
        else
        {
            throw new NoPermissionException();
        }

        return Unit.Value;
    }
}
