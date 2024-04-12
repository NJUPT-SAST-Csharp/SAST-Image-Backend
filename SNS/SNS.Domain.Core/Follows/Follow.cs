namespace SNS.Domain.Follows;

internal sealed record class Follow
{
    public UserId Follower { get; }

    public UserId Following { get; }
}
