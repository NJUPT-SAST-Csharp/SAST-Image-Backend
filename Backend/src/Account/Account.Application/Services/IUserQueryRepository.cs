using Account.Application.Queries;
using Identity;

namespace Account.Application.Services;

public interface IUserQueryRepository
{
    public Task<UserBriefInfoDto?> GetUserBriefInfoAsync(
        string username,
        CancellationToken cancellationToken = default
    );

    public Task<UserBriefInfoDto?> GetUserBriefInfoAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    );

    public Task<UserDetailedInfoDto?> GetUserDetailedInfoAsync(
        string username,
        CancellationToken cancellationToken = default
    );

    public Task<UserDetailedInfoDto?> GetUserDetailedInfoAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    );
}
