using Shared.Primitives.Query;
using SNS.Domain;

namespace SNS.Application.GetFollowCount
{
    public sealed class GetFollowCountQuery(long id) : IQueryRequest<FollowCountDto>
    {
        public UserId UserId { get; } = new(id);
    }
}
