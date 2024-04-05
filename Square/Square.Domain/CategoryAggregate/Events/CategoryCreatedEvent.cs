using Shared.Primitives.DomainEvent;
using Square.Domain.CategoryAggregate.CategoryEntity;

namespace Square.Domain.CategoryAggregate.Events
{
    public sealed class CategoryCreatedEvent(CategoryId categoryId, CategoryName name)
        : IDomainEvent
    {
        public CategoryId CategoryId { get; } = categoryId;
        public CategoryName Name { get; } = name;
    }
}
