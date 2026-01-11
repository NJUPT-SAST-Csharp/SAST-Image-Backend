using Domain.Shared;
using Domain.UserAggregate;
using Domain.UserAggregate.UserEntity;
using Infrastructure.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UserServices.Domain;

internal sealed class UserDomainRepository(DomainDbContext context) : IUserRepository
{
    private readonly DomainDbContext _context = context;

    public async Task AddAsync(User entity, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(entity, cancellationToken);
    }

    public async Task<User> GetAsync(UserId id, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new EntityNotFoundException();
    }

    public Task<User?> GetOrDefaultAsync(Username username, CancellationToken cancellationToken)
    {
        return _context
            .Users.FromSql($"SELECT * FROM domain.users WHERE username ILIKE {username.Value}")
            .FirstOrDefaultAsync(cancellationToken);
    }
}
