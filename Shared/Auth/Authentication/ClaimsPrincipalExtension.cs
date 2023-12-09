﻿using System.Security.Claims;

namespace Auth.Authentication
{
    public static class ClaimsPrincipalExtension
    {
        public static bool TryFetchClaim(this ClaimsPrincipal user, string claim, out string? value)
        {
            var c = user.FindFirst(claim);
            value = c?.Value;
            return c is not null;
        }

        public static bool TryFetchId(this ClaimsPrincipal user, out long id)
        {
            if (user.TryFetchClaim("Id", out string? claim))
            {
                var result = long.TryParse(claim, out id);
                return result;
            }
            id = 0;
            return false;
        }

        public static bool TryFetchId(this ClaimsPrincipal user, out int id)
        {
            if (user.TryFetchClaim("Id", out string? claim))
            {
                var result = int.TryParse(claim, out id);
                return result;
            }
            id = 0;
            return false;
        }

        public static bool TryFetchId(this ClaimsPrincipal user, out Guid id)
        {
            if (user.TryFetchClaim("Id", out string? claim))
            {
                var result = Guid.TryParse(claim, out id);
                return result;
            }
            id = Guid.Empty;
            return false;
        }

        public static bool TryFetchId(this ClaimsPrincipal user, out string? id)
        {
            return user.TryFetchClaim("Id", out id);
        }

        public static bool TryFetchUsername(this ClaimsPrincipal user, out string? username)
        {
            return user.TryFetchClaim("Username", out username);
        }

        public static bool HasRole(this ClaimsPrincipal user, string role)
        {
            foreach (var r in user.FindAll("Roles"))
            {
                if (r is { } roleClaim && roleClaim.Value == role)
                    return true;
            }
            return false;
        }

        public static bool HasClaim(this ClaimsPrincipal user, string claim)
        {
            var result = user.FindFirst(claim);
            return result is not null;
        }
    }
}
