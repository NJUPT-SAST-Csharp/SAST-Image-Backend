using System.Security.Claims;
using Domain.UserAggregate.UserEntity;

namespace Domain.Shared;

public readonly record struct Actor
{
    public readonly UserId Id { get; init; }
    public readonly bool IsAuthenticated { get; init; }
    public readonly bool IsAdmin { get; init; }

    public Actor(ClaimsPrincipal user)
    {
        IsAuthenticated = user.TryFetchId(out long id);
        Id = new(id);
        if (IsAuthenticated)
        {
            IsAdmin = user.HasRole(Role.Admin);
        }
    }

    public static implicit operator Actor(ClaimsPrincipal user) => new(user);
}

public static class ClaimsPrincipalExtensions
{
    public static bool TryFetchClaim(this ClaimsPrincipal user, string claim, out string? value)
    {
        var c = user.FindFirst(claim);
        value = c?.Value;
        return c is not null;
    }

    public static bool TryFetchId(this ClaimsPrincipal user, out long id)
    {
        if (user.TryFetchClaim("id", out string? claim))
        {
            bool result = long.TryParse(claim, out id);
            return result;
        }
        id = 0;
        return false;
    }

    public static bool HasRole(this ClaimsPrincipal user, Role role)
    {
        foreach (var r in user.FindAll("role"))
        {
            if (
                r is { } roleClaim
                && string.Equals(
                    roleClaim.Value,
                    role.ToString(),
                    StringComparison.InvariantCultureIgnoreCase
                )
            )
                return true;
        }
        return false;
    }
}
