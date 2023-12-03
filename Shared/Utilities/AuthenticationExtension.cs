using System.Security.Claims;

namespace Utilities
{
    public static class AuthenticationExtension
    {
        public static bool TryFetchId(this ClaimsPrincipal user, out long id)
        {
            if (TryParseClaim(user, out string? claim))
            {
                var result = long.TryParse(claim, out id);
                return result;
            }
            id = 0;
            return false;
        }

        public static bool TryFetchId(this ClaimsPrincipal user, out int id)
        {
            if (TryParseClaim(user, out string? claim))
            {
                var result = int.TryParse(claim, out id);
                return result;
            }
            id = 0;
            return false;
        }

        public static bool TryFetchId(this ClaimsPrincipal user, out Guid id)
        {
            if (TryParseClaim(user, out string? claim))
            {
                var result = Guid.TryParse(claim, out id);
                return result;
            }
            id = Guid.Empty;
            return false;
        }

        public static bool TryFetchId(this ClaimsPrincipal user, out string? id)
        {
            return TryParseClaim(user, out id);
        }

        private static bool TryParseClaim(ClaimsPrincipal user, out string? id)
        {
            var claim = user.FindFirst("id");
            if (user.Identity is null || !user.Identity.IsAuthenticated || claim is null)
            {
                id = null;
                return false;
            }
            id = claim.Value;
            return true;
        }
    }
}
