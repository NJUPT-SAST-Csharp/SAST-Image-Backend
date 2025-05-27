using Identity;
using Mediator;
using SastImg.Application.ImageServices.GetAlbumImages;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.ImageServices.GetRemovedImages;

public sealed record class GetRemovedImagesQuery(AlbumId AlbumId, Requester Requester)
    : IQuery<IEnumerable<AlbumImageDto>>;
