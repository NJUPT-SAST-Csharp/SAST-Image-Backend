using Domain.CategoryAggregate.CategoryEntity;
using Domain.CategoryAggregate.Services;
using Domain.Shared;
using Mediator;

namespace Domain.CategoryAggregate.Commands;

public sealed record class CreateCategoryCommand(
    CategoryName Name,
    CategoryDescription Description,
    Actor Actor
) : ICommand<CategoryId>;

internal sealed class CreateCategoryCommandHandler(
    ICategoryRepository repository,
    ICategoryNameUniquenessChecker checker
) : ICommandHandler<CreateCategoryCommand, CategoryId>
{
    public async ValueTask<CategoryId> Handle(
        CreateCategoryCommand command,
        CancellationToken cancellationToken
    )
    {
        await checker.CheckAsync(command.Name, cancellationToken);

        var category = new Category(command);
        await repository.AddAsync(category, cancellationToken);

        return category.Id;
    }
}
