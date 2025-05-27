namespace Account.Domain.UserEntity.Services;

public interface IUserUniquenessChecker
{
    public Task<bool> CheckEmailExistenceAsync(
        string email,
        CancellationToken cancellationToken = default
    );
    public Task<bool> CheckUsernameExistenceAsync(
        string username,
        CancellationToken cancellationToken = default
    );
}
