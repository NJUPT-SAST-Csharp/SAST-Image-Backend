using Identity;
using Mediator;
using Microsoft.AspNetCore.Http;

namespace Account.Application.UserServices.GetUserBriefInfo;

public sealed class GetUserInfoQuery(string? username, long? id, bool isDetailed) : IQuery<IResult>
{
    public string? Username { get; } = username;
    public UserId? Id { get; } = id.HasValue ? new(id.Value) : null;
    public bool IsDetailed { get; } = isDetailed;
}
