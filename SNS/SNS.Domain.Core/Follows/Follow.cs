using Identity;

namespace SNS.Domain.Follows;

internal sealed record class Follow
{
    public UserId Follower { get; init; }

    public UserId Following { get; init; }
}
