using Identity;
using Mediator;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Domain.AlbumAggregate.ImageEntity.Commands;

public sealed record class RemoveImageCommand(AlbumId AlbumId, ImageId ImageId, Requester Requester)
    : ICommand;
