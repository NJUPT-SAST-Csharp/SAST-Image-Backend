using Domain.CategoryAggregate.CategoryEntity;
using Domain.Event;

namespace Domain.CategoryAggregate.Events;

public sealed record class CategoryCreatedEvent(
    CategoryId Id,
    CategoryName Name,
    CategoryDescription Description
) : IDomainEvent { }
