using Domain.AlbumAggregate.AlbumEntity;
using Domain.CategoryAggregate.CategoryEntity;
using Domain.Event;
using Domain.UserAggregate.UserEntity;

namespace Domain.AlbumAggregate.Events;

public sealed record AlbumCreatedEvent(
    AlbumId AlbumId,
    UserId AuthorId,
    CategoryId CategoryId,
    AlbumTitle Title,
    AlbumDescription Description,
    AccessLevel AccessLevel
) : IDomainEvent { }
