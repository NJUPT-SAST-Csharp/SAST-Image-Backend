using Shared.Primitives.Query;

namespace Account.Application.UserServices.GetUserDetailedInfo
{
    public sealed class GetUserDetailedInfoQuery(string username)
        : IQueryRequest<UserDetailedInfoDto>
    {
        public string Username { get; } = username;
    }
}
