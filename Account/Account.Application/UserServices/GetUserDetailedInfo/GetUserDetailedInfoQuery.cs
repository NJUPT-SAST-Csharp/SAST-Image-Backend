using Mediator;
using Microsoft.AspNetCore.Http;

namespace Account.Application.UserServices.GetUserDetailedInfo;

public sealed class GetUserDetailedInfoQuery(string username) : IQuery<IResult>
{
    public string Username { get; } = username;
}
