using System.Security.Claims;
using SastImg.Application.SeedWorks;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using Shared.Primitives.Query;

namespace SastImg.Application.ImageServices.GetImages
{
    public sealed class GetImagesQuery(long albumId, int page, ClaimsPrincipal user)
        : IQueryRequest<IEnumerable<AlbumImageDto>>
    {
        public int Page { get; } = page;
        public AlbumId AlbumId { get; } = new(albumId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
