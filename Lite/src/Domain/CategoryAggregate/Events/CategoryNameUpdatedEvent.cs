using Domain.CategoryAggregate.CategoryEntity;
using Domain.Event;

namespace Domain.CategoryAggregate.Events;

public sealed record class CategoryNameUpdatedEvent(CategoryId Id, CategoryName Name)
    : IDomainEvent;
