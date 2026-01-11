using Domain.UserAggregate.UserEntity;

namespace Application.UserServices;

public interface IUserModelRepository
{
    public Task<UserModel> GetAsync(UserId id, CancellationToken cancellationToken);

    public Task AddAsync(UserModel model, CancellationToken cancellationToken);
}
