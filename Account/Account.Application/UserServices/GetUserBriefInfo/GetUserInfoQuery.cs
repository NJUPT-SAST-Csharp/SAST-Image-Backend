using Account.Domain.UserEntity;
using Microsoft.AspNetCore.Http;
using Shared.Primitives.Query;

namespace Account.Application.UserServices.GetUserBriefInfo
{
    public sealed class GetUserInfoQuery(string? username, long? id, bool isDetailed)
        : IQueryRequest<IResult>
    {
        public string? Username { get; } = username;
        public UserId? Id { get; } = id.HasValue ? new(id.Value) : null;
        public bool IsDetailed { get; } = isDetailed;
    }
}
