namespace Account.Entity.User.Repositories
{
    public interface IUserQueryRepository
    {
        public Task<User?> GetUserByIdAsync(
            long userId,
            CancellationToken cancellationToken = default
        );

        public Task<User?> GetUserByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default
        );
    }
}
