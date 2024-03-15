using Account.Application.UserServices.GetUserBriefInfo;
using Account.Application.UserServices.GetUserDetailedInfo;

namespace Account.Application.UserServices
{
    public interface IUserQueryRepository
    {
        public Task<UserBriefInfoDto?> GetUserBriefInfoAsync(
            string username,
            CancellationToken cancellationToken = default
        );

        public Task<UserDetailedInfoDto?> GetUserDetailedInfoAsync(
            string username,
            CancellationToken cancellationToken = default
        );
    }
}
