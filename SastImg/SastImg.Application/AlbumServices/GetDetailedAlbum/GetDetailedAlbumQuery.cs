using Identity;
using Mediator;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumServices.GetDetailedAlbum;

public sealed record class GetDetailedAlbumQuery(AlbumId AlbumId, Requester Requester)
    : IQuery<DetailedAlbumDto?>;
