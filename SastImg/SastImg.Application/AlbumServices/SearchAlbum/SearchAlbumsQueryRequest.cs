using System.Security.Claims;
using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Application.SeedWorks;
using Shared.Primitives.Request;

namespace SastImg.Application.AlbumServices.SearchAlbum
{
    public sealed class SearchAlbumsQueryRequest(
        long categoryId,
        string title,
        int page,
        ClaimsPrincipal user
    ) : IQueryRequest<IEnumerable<AlbumDto>>
    {
        public int Page { get; } = page;
        public string Title { get; } = title;
        public long CategoryId { get; } = categoryId;
        public RequesterInfo Requester { get; } = new(user);
    }
}
