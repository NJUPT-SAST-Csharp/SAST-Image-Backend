using Domain.CategoryAggregate.CategoryEntity;
using Domain.Shared;
using Mediator;

namespace Domain.CategoryAggregate.Commands;

public sealed record class UpdateCategoryDescriptionCommand(
    CategoryId Id,
    CategoryDescription Description,
    Actor Actor
) : ICommand;

internal sealed class UpdateCategoryDescriptionCommandHandler(ICategoryRepository repository)
    : ICommandHandler<UpdateCategoryDescriptionCommand>
{
    public async ValueTask<Unit> Handle(
        UpdateCategoryDescriptionCommand command,
        CancellationToken cancellationToken
    )
    {
        var category = await repository.GetAsync(command.Id, cancellationToken);

        category.UpdateDescription(command);

        return Unit.Value;
    }
}
