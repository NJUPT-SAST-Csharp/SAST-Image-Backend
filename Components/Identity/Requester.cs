using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Identity;

public sealed class Requester : IEquatable<Requester>
{
    public const string UserIdClaimType = nameof(UserId);
    public const string RolesClaimType = nameof(Roles);

    public static readonly Requester Anonymous = new();

    private Requester() { }

    public Requester(ClaimsPrincipal user)
    {
        if (
            user.TryFetchClaim(UserIdClaimType, out string? idValue) is false
            || user.TryFetchClaim(RolesClaimType, out string? roleValue) is false
            || long.TryParse(idValue, out long id) is false
            || Enum.TryParse<Roles>(roleValue, true, out var role) is false
        )
        {
            return;
        }

        Id = new(id);
        Role = role;
    }

    public UserId Id { get; } = new() { Value = -1 };
    public Roles Role { get; } = Roles.NONE;
    public bool IsAuthenticated => Role != Roles.NONE;
    public bool IsAdmin => (Role & Roles.ADMIN) != 0;

    public bool Equals(Requester? other) => Id == other?.Id;

    public override bool Equals(object? obj) => obj is Requester requester && Equals(requester);

    public static bool operator ==(Requester left, Requester right) => left.Equals(right);

    public static bool operator !=(Requester left, Requester right) => !(left == right);

    public override int GetHashCode() => Id.GetHashCode();

    public static implicit operator Requester(ClaimsPrincipal user) => new(user);
}

public static class ClaimsPrincipalExtensions
{
    public static bool TryFetchClaim(
        this ClaimsPrincipal user,
        string claim,
        [NotNullWhen(true)] out string? value
    )
    {
        var c = user.FindFirst(claim);
        value = c?.Value;
        return c is not null;
    }
}
