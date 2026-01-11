using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Event;
using Domain.Shared;
using Domain.UserAggregate.UserEntity;

namespace Domain.AlbumAggregate.Events;

public sealed record class ImageAddedEvent(
    AlbumId Album,
    ImageId ImageId,
    UserId AuthorId,
    ImageTitle Title,
    ImageTags Tags,
    AccessLevel AccessLevel,
    Collaborators Collaborators,
    IImageFile ImageFile,
    DateTime CreatedAt,
    UserId Uploader
) : IDomainEvent { }
