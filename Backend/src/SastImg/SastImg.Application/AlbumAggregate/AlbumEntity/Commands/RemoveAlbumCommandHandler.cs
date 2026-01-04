using Mediator;
using Primitives;
using SastImg.Domain.AlbumAggregate;
using SastImg.Domain.AlbumAggregate.AlbumEntity.Commands;

namespace SastImg.Application.AlbumAggregate.AlbumEntity.Commands;

public sealed class RemoveAlbumCommandHandler(IAlbumRepository repository, IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveAlbumCommand>
{
    private readonly IAlbumRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async ValueTask<Unit> Handle(
        RemoveAlbumCommand command,
        CancellationToken cancellationToken
    )
    {
        var album = await _repository.GetAlbumAsync(command.AlbumId, cancellationToken);

        album.Remove(command);

        return Unit.Value;
    }
}
