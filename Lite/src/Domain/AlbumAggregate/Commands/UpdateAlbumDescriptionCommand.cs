using Domain.AlbumAggregate.AlbumEntity;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class UpdateAlbumDescriptionCommand(
    AlbumId Album,
    AlbumDescription Description,
    Actor Actor
) : ICommand { }

internal sealed class UpdateAlbumDescriptionCommandHandler(IAlbumRepository repository)
    : ICommandHandler<UpdateAlbumDescriptionCommand>
{
    public async ValueTask<Unit> Handle(
        UpdateAlbumDescriptionCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(request.Album, cancellationToken);

        album.UpdateDescription(request);

        return Unit.Value;
    }
}
