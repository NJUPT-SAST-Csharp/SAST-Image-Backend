namespace Account.Entity.UserEntity.Repositories
{
    public interface IUserCommandRepository
    {
        public Task<bool> CreateUserAsync(User user, CancellationToken cancellationToken = default);
    }
}
