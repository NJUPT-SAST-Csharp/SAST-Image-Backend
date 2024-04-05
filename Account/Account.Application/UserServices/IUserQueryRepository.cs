using Account.Application.UserServices.GetUserBriefInfo;
using Account.Application.UserServices.GetUserDetailedInfo;
using Account.Domain.UserEntity;

namespace Account.Application.UserServices
{
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
}
