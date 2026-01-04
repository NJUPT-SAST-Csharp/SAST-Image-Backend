using Identity;
using Mediator;

namespace SastImg.Application.AlbumAggregate.GetRemovedAlbums;

public sealed record class GetRemovedAlbumsQuery(UserId AuthorId, Requester Requester)
    : IQuery<IEnumerable<RemovedAlbumDto>>;
