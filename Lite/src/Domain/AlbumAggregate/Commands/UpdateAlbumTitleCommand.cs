using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.Services;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class UpdateAlbumTitleCommand(AlbumId Album, AlbumTitle Title, Actor Actor)
    : ICommand { }

internal sealed class UpdateAlbumTitleCommandHandler(
    IAlbumRepository repository,
    IAlbumTitleUniquenessChecker checker
) : ICommandHandler<UpdateAlbumTitleCommand>
{
    private readonly IAlbumTitleUniquenessChecker _checker = checker;

    public async ValueTask<Unit> Handle(
        UpdateAlbumTitleCommand command,
        CancellationToken cancellationToken = default
    )
    {
        await _checker.CheckAsync(command.Title, cancellationToken);

        var album = await repository.GetAsync(command.Album, cancellationToken);

        album.UpdateTitle(command);

        return Unit.Value;
    }
}
