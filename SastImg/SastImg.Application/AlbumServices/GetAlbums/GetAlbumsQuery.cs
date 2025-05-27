using Identity;
using Mediator;
using SastImg.Domain.CategoryEntity;

namespace SastImg.Application.AlbumServices.GetAlbums;

public sealed record class GetAlbumsQuery(CategoryId CategoryId, int Page, Requester Requester)
    : IQuery<IEnumerable<AlbumDto>>;
