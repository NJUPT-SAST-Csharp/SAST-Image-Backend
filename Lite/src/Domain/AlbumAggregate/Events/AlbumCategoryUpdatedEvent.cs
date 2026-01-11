using Domain.AlbumAggregate.AlbumEntity;
using Domain.CategoryAggregate.CategoryEntity;
using Domain.Event;

namespace Domain.AlbumAggregate.Events;

public sealed record class AlbumCategoryUpdatedEvent(AlbumId Album, CategoryId Category)
    : IDomainEvent { }
