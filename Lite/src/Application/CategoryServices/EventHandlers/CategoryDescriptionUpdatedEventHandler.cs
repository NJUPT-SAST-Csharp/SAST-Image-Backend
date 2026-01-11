using Domain.CategoryAggregate.Events;
using Domain.Core.Event;

namespace Application.CategoryServices.EventHandlers;

internal sealed class CategoryDescriptionUpdatedEventHandler(ICategoryModelRepository repository)
    : IDomainEventHandler<CategoryDescriptionUpdatedEvent>
{
    public async ValueTask Handle(
        CategoryDescriptionUpdatedEvent e,
        CancellationToken cancellationToken
    )
    {
        var category = await repository.GetAsync(e.Id, cancellationToken);

        category.UpdateDescription(e);
    }
}
