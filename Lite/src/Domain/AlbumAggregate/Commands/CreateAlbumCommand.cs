using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.Services;
using Domain.CategoryAggregate.CategoryEntity;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class CreateAlbumCommand(
    AlbumTitle Title,
    AlbumDescription Description,
    AccessLevel AccessLevel,
    CategoryId CategoryId,
    Actor Actor
) : ICommand<AlbumId> { }

internal sealed class CreateAlbumCommandHandler(
    IAlbumRepository repository,
    IAlbumTitleUniquenessChecker titleChecker,
    ICategoryExistenceChecker categoryChecker
) : ICommandHandler<CreateAlbumCommand, AlbumId>
{
    public async ValueTask<AlbumId> Handle(
        CreateAlbumCommand command,
        CancellationToken cancellationToken
    )
    {
        var id = await Album.CreateAsync(
            command,
            categoryChecker,
            titleChecker,
            repository,
            cancellationToken
        );

        return id;
    }
}
