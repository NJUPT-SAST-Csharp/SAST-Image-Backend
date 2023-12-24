using System.Security.Claims;
using Auth.Authentication;
using Auth.Authorization;

namespace SastImg.Application
{
    public sealed class RequesterInfo
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

        public bool IsAuthenticated { get; }
        public long Id { get; }
        public bool IsAdmin { get; }
    }
}
