using Microsoft.AspNetCore.Http;
using Shared.Primitives.Query;

namespace Account.Application.UserServices.GetUserBriefInfo
{
    public sealed class GetUserBriefInfoQuery(string username) : IQueryRequest<IResult>
    {
        public string Username { get; } = username;
    }
}
