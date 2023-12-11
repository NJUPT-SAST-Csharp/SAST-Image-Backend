namespace Account.Entity.UserEntity.Repositories
{
    public interface IUserCheckRepository
    {
        public Task<bool> CheckEmailExistenceAsync(
            string email,
            CancellationToken cancellationToken = default
        );
        public Task<bool> CheckUsernameExistenceAsync(
            string email,
            CancellationToken cancellationToken = default
        );
    }
}
