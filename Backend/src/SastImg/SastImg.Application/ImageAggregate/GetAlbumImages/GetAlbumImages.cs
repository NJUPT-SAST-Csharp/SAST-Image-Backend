using Identity;
using Mediator;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.ImageServices.GetAlbumImages;

public sealed record class GetAlbumImages(AlbumId AlbumId, int Page, Requester Requester)
    : IQuery<IEnumerable<AlbumImageDto>>;
