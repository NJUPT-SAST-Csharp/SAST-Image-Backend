using Application.UserServices;
using Domain.Shared;
using Domain.UserAggregate.UserEntity;
using Infrastructure.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UserServices.Application;

internal sealed class UserModelRepository(QueryDbContext context) : IUserModelRepository
{
    public async Task AddAsync(UserModel entity, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(UserId id, CancellationToken cancellationToken = default)
    {
        var user = await GetAsync(id, cancellationToken);
        context.Users.Remove(user);
    }

    public async Task<UserModel> GetAsync(UserId id, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(
                user => user.Id == id.Value,
                cancellationToken
            ) ?? throw new EntityNotFoundException();
    }
}
