using Domain.UserAggregate.Exceptions;
using Domain.UserAggregate.Services;
using Domain.UserAggregate.UserEntity;
using Infrastructure.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UserServices.Domain;

internal sealed class UsernameUniquenessChecker(DomainDbContext context)
    : IUsernameUniquenessChecker
{
    public async Task CheckAsync(Username username, CancellationToken cancellationToken = default)
    {
        bool isExisting = await context
            .Users.FromSql($"Select 1 From domain.users WHERE username ILIKE {username.Value}")
            .AsNoTracking()
            .AnyAsync(cancellationToken);

        if (isExisting)
            throw new UsernameDuplicateException(username);
    }
}
