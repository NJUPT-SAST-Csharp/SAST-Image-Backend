using Domain.CategoryAggregate.CategoryEntity;
using Domain.CategoryAggregate.Events;
using Domain.Core.Event;

namespace Application.CategoryServices.EventHandlers;

internal sealed class CategoryNameUpdatedEventHandler(
    ICategoryModelRepository repository
) : IDomainEventHandler<CategoryNameUpdatedEvent>
{
    public async ValueTask Handle(CategoryNameUpdatedEvent e, CancellationToken cancellationToken)
    {
        var category = await repository.GetAsync(e.Id, cancellationToken);

        category.UpdateName(e);
    }
}
