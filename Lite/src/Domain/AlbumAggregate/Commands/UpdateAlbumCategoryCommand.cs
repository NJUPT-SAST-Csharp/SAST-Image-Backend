using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.Services;
using Domain.CategoryAggregate.CategoryEntity;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class UpdateAlbumCategoryCommand(
    AlbumId Album,
    CategoryId Category,
    Actor Actor
) : ICommand { }

internal sealed class UpdateAlbumCategoryCommandHandler(
    IAlbumRepository repository,
    ICategoryExistenceChecker checker
) : ICommandHandler<UpdateAlbumCategoryCommand>
{
    private readonly ICategoryExistenceChecker _checker = checker;

    public async ValueTask<Unit> Handle(
        UpdateAlbumCategoryCommand request,
        CancellationToken cancellationToken
    )
    {
        await _checker.CheckAsync(request.Category, cancellationToken);

        var album = await repository.GetAsync(request.Album, cancellationToken);

        album.UpdateCategory(request);

        return Unit.Value;
    }
}
