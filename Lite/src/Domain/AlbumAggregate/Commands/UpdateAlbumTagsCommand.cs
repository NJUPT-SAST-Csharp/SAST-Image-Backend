using Domain.AlbumAggregate.AlbumEntity;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class UpdateAlbumTagsCommand(AlbumId Id, AlbumTags Tags, Actor Actor)
    : ICommand;

internal sealed class UpdateAlbumTagsCommandHandler(IAlbumRepository repository)
    : ICommandHandler<UpdateAlbumTagsCommand>
{
    public async ValueTask<Unit> Handle(
        UpdateAlbumTagsCommand command,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(command.Id, cancellationToken);
        album.UpdateTags(command);

        return Unit.Value;
    }
}
