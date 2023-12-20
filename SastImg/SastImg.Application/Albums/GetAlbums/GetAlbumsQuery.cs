using System.Security.Claims;
using Auth.Authentication;
using Auth.Authorization;
using SastImg.Application.Albums.Dtos;
using Shared.Primitives.Request;

namespace SastImg.Application.Albums.GetAlbums
{
    public sealed class GetAlbumsQuery : IQueryRequest<IEnumerable<AlbumDto>>
    {
        public GetAlbumsQuery(int page, long authorId, ClaimsPrincipal user)
        {
            Page = page;
            AuthorId = authorId;
            IsAuthenticated = user.Identity is { } auth && auth.IsAuthenticated;
            if (IsAuthenticated)
            {
                user.TryFetchId(out long id);
                RequesterId = id;
                Roles = user.GetRoles();
            }
            else
            {
                Roles = [];
            }
        }

        public bool IsAuthenticated = false;
        public int Page { get; } = 0;
        public long AuthorId { get; } = 0;
        public long RequesterId { get; } = 0;
        public IEnumerable<AuthorizationRole> Roles { get; }
    }
}
