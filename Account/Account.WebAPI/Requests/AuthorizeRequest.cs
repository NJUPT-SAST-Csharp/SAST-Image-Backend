using Identity;

namespace Account.WebAPI.Requests;

public readonly struct AuthorizeRequest
{
    public readonly long UserId { get; init; }
    public readonly Roles[] Roles { get; init; }
}
