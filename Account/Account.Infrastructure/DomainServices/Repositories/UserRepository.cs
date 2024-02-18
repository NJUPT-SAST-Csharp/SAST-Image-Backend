using Account.Domain.UserEntity;
using Account.Domain.UserEntity.Services;
using Account.Infrastructure.Persistence;
using Exceptions.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.DomainServices.Repositories
{
    public sealed class UserRepository(AccountDbContext context) : IUserRepository
    {
        private readonly AccountDbContext _context = context;

        public async Task<UserId> AddNewUserAsync(
            User user,
            CancellationToken cancellationToken = default
        )
        {
            var entity = await _context.Users.AddAsync(user, cancellationToken);
            return entity.Entity.Id;
        }

        public async Task<User> GetUserByEmailAsync(
            string email,
            CancellationToken cancellationToken = default
        )
        {
            email = email.ToUpperInvariant();
            var user = await _context
                .Users.Include("_roles")
                .FirstOrDefaultAsync(
                    u => EF.Property<string>(u, "_email") == email,
                    cancellationToken
                );

            if (user is null)
            {
                throw new DbNotFoundException(nameof(User), email);
            }

            return user;
        }

        public async Task<User> GetUserByIdAsync(
            UserId id,
            CancellationToken cancellationToken = default
        )
        {
            var user = await _context
                .Users.Include("_roles")
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (user is null)
            {
                throw new DbNotFoundException(nameof(User), id.Value.ToString());
            }

            return user;
        }

        public async Task<User> GetUserByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            username = username.ToUpperInvariant();

            var user = await _context
                .Users.Include("_roles")
                .FirstOrDefaultAsync(
                    u => EF.Property<string>(u, "_normalizedUsername") == username,
                    cancellationToken
                );

            if (user is null)
            {
                throw new DbNotFoundException(nameof(User), username);
            }

            return user;
        }
    }
}
