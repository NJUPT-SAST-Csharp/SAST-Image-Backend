namespace SNS.Domain.UserEntity
{
    public interface IUserRepository
    {
        public Task<UserId> AddNewUserAsync(
            User user,
            CancellationToken cancellationToken = default
        );

        public Task<User> GetUserAsync(
            UserId userId,
            CancellationToken cancellationToken = default
        );
    }
}
