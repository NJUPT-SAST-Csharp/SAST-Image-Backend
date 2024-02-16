using SNS.Domain.UserEntity;

namespace SNS.Application.UserServices.GetUser
{
    public interface IUserQueryRepository
    {
        public Task<UserDto?> GetUserAsync(
            UserId userId,
            CancellationToken cancellationToken = default
        );
    }
}
