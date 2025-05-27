using Identity;
using Mediator;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.AlbumAggregate.ImageEntity;

namespace SastImg.Application.ImageServices.RemoveImage;

public sealed record class RemoveImageCommand(AlbumId AlbumId, ImageId ImageId, Requester Requester)
    : ICommand;
