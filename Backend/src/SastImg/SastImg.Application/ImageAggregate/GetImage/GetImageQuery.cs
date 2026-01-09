using Identity;
using Mediator;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.AlbumAggregate.ImageEntity;

namespace SastImg.Application.ImageServices.GetImage;

public sealed record class GetImageQuery(AlbumId AlbumId, ImageId ImageId, Requester Requester)
    : IQuery<DetailedImageDto?>;
