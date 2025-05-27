using Identity;
using Mediator;

namespace SastImg.Application.AlbumServices.GetUserAlbums;

public sealed record class GetUserAlbumsQuery(UserId AuthorId, Requester Requester)
    : IQuery<IEnumerable<UserAlbumDto>>;
