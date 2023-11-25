namespace Account.Application.Users
{
    public interface IUserRepository
    {
        public Task ModifyUserProfileAsync(CancellationToken cancellationToken = default);

        public Task CreateUserAsync(CancellationToken cancellationToken = default);
    }
}
