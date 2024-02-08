using Account.Entity.UserEntity;

namespace Account.Application.Endpoints.UserEndpoints.Query
{
    public sealed class QueryUserDto
    {
        public static QueryUserDto? FromUser(User? user)
        {
            if (user is null)
                return null;
            return new QueryUserDto
            {
                Id = user.Id,
                Username = user.Username,
                UsernameNormalized = user.UsernameNormalized,
                Email = user.Email
            };
        }

        public long Id { get; init; }
        public string Username { get; init; }
        public string UsernameNormalized { get; init; }
        public string Email { get; init; }
    }
}
