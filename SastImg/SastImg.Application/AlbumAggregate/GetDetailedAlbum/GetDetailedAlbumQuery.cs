using Identity;
using Mediator;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumAggregate.GetDetailedAlbum;

public sealed record class GetDetailedAlbumQuery(AlbumId AlbumId, Requester Requester)
    : IQuery<DetailedAlbumDto?>;
