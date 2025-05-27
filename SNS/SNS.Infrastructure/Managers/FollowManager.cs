using Identity;
using Microsoft.EntityFrameworkCore;
using SNS.Domain.Follows;
using SNS.Infrastructure.Persistence;

namespace SNS.Infrastructure.Managers;

internal sealed class FollowManager(SNSDbContext context) : IFollowManager
{
    private readonly SNSDbContext _context = context;

    public async Task FollowAsync(
        UserId follower,
        UserId target,
        CancellationToken cancellationToken = default
    )
    {
        await _context.Follows.AddAsync(
            new() { Follower = follower, Following = target },
            cancellationToken
        );
    }

    public Task<Follow?> GetFollowAsync(
        UserId follower,
        UserId target,
        CancellationToken cancellationToken = default
    )
    {
        return _context.Follows.FirstOrDefaultAsync(
            f => f.Follower == follower && f.Following == target,
            cancellationToken
        );
    }

    public Task UnfollowAsync(Follow follow, CancellationToken cancellationToken = default)
    {
        _context.Follows.Remove(follow);
        return Task.CompletedTask;
    }
}
