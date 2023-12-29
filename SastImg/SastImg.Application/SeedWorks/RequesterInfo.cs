using System.Security.Claims;
using Auth.Authentication;
using Auth.Authorization;

namespace SastImg.Application.SeedWorks
{
    public readonly struct RequesterInfo
    {
        public RequesterInfo(ClaimsPrincipal user)
        {
            IsAuthenticated = user.TryFetchId(out long id);
            Id = id;
            if (IsAuthenticated)
            {
                IsAdmin = user.HasRole(AuthorizationRole.Admin);
            }
        }

        public readonly bool IsAuthenticated { get; }
        public readonly long Id { get; }
        public readonly bool IsAdmin { get; }
    }
}
