using Shared.Primitives.Query;

namespace Account.Application.UserServices.GetUserBriefInfo
{
    public sealed class GetUserBriefInfoQuery(string username) : IQueryRequest<UserBriefInfoDto>
    {
        public string Username { get; } = username;
    }
}
