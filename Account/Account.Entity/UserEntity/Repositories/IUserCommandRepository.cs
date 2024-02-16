namespace Account.Entity.UserEntity.Repositories
{
    public interface IUserCommandRepository
    {
        public Task CreateUserAsync(User user, CancellationToken cancellationToken = default);
    }
}
