using Domain.CategoryAggregate.CategoryEntity;
using Domain.Shared;
using Mediator;

namespace Domain.CategoryAggregate.Commands;

public sealed record class UpdateCategoryNameCommand(CategoryId Id, CategoryName Name, Actor Actor)
    : ICommand;

internal sealed class UpdateCategoryNameCommandHandler(ICategoryRepository repository)
    : ICommandHandler<UpdateCategoryNameCommand>
{
    public async ValueTask<Unit> Handle(
        UpdateCategoryNameCommand command,
        CancellationToken cancellationToken
    )
    {
        var category = await repository.GetAsync(command.Id, cancellationToken);

        category.UpdateName(command);

        return Unit.Value;
    }
}
