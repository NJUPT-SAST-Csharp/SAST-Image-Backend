using Application.Shared;
using Application.UserServices.Queries;
using Infrastructure.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UserServices.Application;

internal sealed class UserQueryRepository(QueryDbContext context)
    : IQueryRepository<UsernameExistenceQuery, UsernameExistence>,
        IQueryRepository<UserProfileQuery, UserProfileDto?>
{
    public async Task<UsernameExistence> GetOrDefaultAsync(
        UsernameExistenceQuery query,
        CancellationToken cancellationToken = default
    )
    {
        bool isExist = await context
            .Users.AsNoTracking()
            .AnyAsync(
                user => EF.Functions.ILike(user.Username, query.Username.Value),
                cancellationToken
            );

        return new(isExist);
    }

    public Task<UserProfileDto?> GetOrDefaultAsync(
        UserProfileQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return context
            .Users.AsNoTracking()
            .Where(context => context.Id == query.User.Value)
            .Select(user => new UserProfileDto(
                user.Id,
                user.Username,
                user.Nickname,
                user.Biography
            ))
            .SingleOrDefaultAsync(cancellationToken);
    }
}
