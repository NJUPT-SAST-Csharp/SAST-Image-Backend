using Domain.CategoryAggregate.Events;
using Domain.Core.Event;

namespace Application.CategoryServices.EventHandlers;

internal sealed class CategoryCreatedEventHandler(ICategoryModelRepository repository)
    : IDomainEventHandler<CategoryCreatedEvent>
{
    public async ValueTask Handle(CategoryCreatedEvent e, CancellationToken cancellationToken)
    {
        CategoryModel category = new(e);

        await repository.AddAsync(category, cancellationToken);
    }
}
