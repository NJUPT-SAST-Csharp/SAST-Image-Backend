namespace Account.Entity.User.Repositories
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
        public Task<bool> CheckSignInAsync(
            string username,
            byte[] password,
            CancellationToken cancellationToken = default
        );
    }
}
