namespace Account.Entity.User.Repositories
{
    public interface IUserCommandRepository
    {
        public Task<bool> CreateUserAsync(User user, CancellationToken cancellationToken = default);
    }
}
