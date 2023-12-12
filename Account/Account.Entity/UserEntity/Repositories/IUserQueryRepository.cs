namespace Account.Entity.UserEntity.Repositories
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

        public Task<User?> GetUserByEmailAsync(
            string email,
            CancellationToken cancellationToken = default
        );

        public Task<User?> GetUserDetailByIdAsync(
            long userId,
            CancellationToken cancellationToken = default
        );
    }
}
