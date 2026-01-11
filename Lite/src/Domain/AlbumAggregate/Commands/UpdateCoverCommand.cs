using Domain.AlbumAggregate.AlbumEntity;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class UpdateCoverCommand(AlbumId Album, IImageFile? CoverImage, Actor Actor)
    : ICommand { }

internal sealed class UpdateCoverCommandHandler(IAlbumRepository repository)
    : ICommandHandler<UpdateCoverCommand>
{
    public async ValueTask<Unit> Handle(
        UpdateCoverCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(request.Album, cancellationToken);

        album.UpdateCover(request);

        return Unit.Value;
    }
}
