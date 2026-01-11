using Domain.CategoryAggregate.CategoryEntity;
using Domain.Event;

namespace Domain.CategoryAggregate.Events;

public sealed record class CategoryDescriptionUpdatedEvent(
    CategoryId Id,
    CategoryDescription Description
) : IDomainEvent { }
