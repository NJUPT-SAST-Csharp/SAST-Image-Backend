using Primitives.DomainEvent;
using Square.Domain.CategoryAggregate.Events;

namespace Square.Application.CategoryServices.EventHandlers
{
    internal sealed class CategoryCreatedEventHandler(ICategoryQueryRepository repository)
        : IDomainEventHandler<CategoryCreatedEvent>
    {
        private readonly ICategoryQueryRepository _repository = repository;

        public Task Handle(CategoryCreatedEvent notification, CancellationToken cancellationToken)
        {
            var category = CategoryModel.CreateNewCategory(notification);

            _repository.AddCategory(category);

            return Task.CompletedTask;
        }
    }
}
