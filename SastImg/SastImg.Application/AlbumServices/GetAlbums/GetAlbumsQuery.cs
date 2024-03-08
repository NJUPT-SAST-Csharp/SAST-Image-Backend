using System.Security.Claims;
using SastImg.Application.SeedWorks;
using SastImg.Domain.CategoryEntity;
using Shared.Primitives.Query;

namespace SastImg.Application.AlbumServices.GetAlbums
{
    public class GetAlbumsQuery(long categoryId, int page, ClaimsPrincipal user)
        : IQueryRequest<IEnumerable<AlbumDto>>
    {
        public CategoryId CategoryId { get; } = new(categoryId);
        public int Page { get; } = page;
        public RequesterInfo Requester { get; } = new(user);
    }
}
