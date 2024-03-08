using Account.Domain.UserEntity;

namespace Account.WebAPI.Requests
{
    public readonly struct AuthorizeRequest
    {
        public readonly long UserId { get; init; }
        public readonly Role[] Roles { get; init; }
    }
}
