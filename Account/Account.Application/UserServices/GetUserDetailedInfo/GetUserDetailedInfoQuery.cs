using Microsoft.AspNetCore.Http;
using Shared.Primitives.Query;

namespace Account.Application.UserServices.GetUserDetailedInfo
{
    public sealed class GetUserDetailedInfoQuery(string username) : IQueryRequest<IResult>
    {
        public string Username { get; } = username;
    }
}
