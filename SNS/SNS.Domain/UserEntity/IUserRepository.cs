namespace SNS.Domain.UserEntity
{
    public interface IUserRepository
    {
        public Task<UserId> AddNewUserAsync(
            User user,
            CancellationToken cancellationToken = default
        );
    }
}
