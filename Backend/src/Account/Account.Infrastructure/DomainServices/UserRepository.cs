using Account.Domain.UserEntity;
using Account.Domain.UserEntity.Services;
using Account.Domain.UserEntity.ValueObjects;
using Account.Infrastructure.Persistence;
using Identity;
using Microsoft.EntityFrameworkCore;
using Primitives.Exceptions;

namespace Account.Infrastructure.DomainServices;

public sealed class UserRepository(AccountDbContext context) : IUserRepository
{
    private readonly AccountDbContext _context = context;

    public async Task<UserId> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Users.AddAsync(user, cancellationToken);
        return entity.Entity.Id;
    }

    public async Task<User> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        EntityNotFoundException.ThrowIf(user is null);

        return user;
    }

    public async Task<User> GetByUsernameAsync(
        Username username,
        CancellationToken cancellationToken = default
    )
    {
        var user = await _context.Users.FirstOrDefaultAsync(
            u => EF.Functions.ILike(u.Username.Value, username.Value),
            cancellationToken
        );

        EntityNotFoundException.ThrowIf(user is null);

        return user;
    }
}
