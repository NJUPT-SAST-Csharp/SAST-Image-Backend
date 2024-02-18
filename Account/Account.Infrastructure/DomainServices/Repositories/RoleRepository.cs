using Account.Domain.RoleEntity;
using Account.Domain.RoleEntity.Services;
using Account.Infrastructure.Persistence;
using Exceptions.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.DomainServices.Repositories
{
    public sealed class RoleRepository(AccountDbContext context) : IRoleRespository
    {
        private readonly AccountDbContext _dbContext = context;

        public async Task<RoleId> AddNewRoleAsync(
            Role role,
            CancellationToken cancellationToken = default
        )
        {
            var entity = await _dbContext.Roles.AddAsync(role, cancellationToken);

            return entity.Entity.Id;
        }

        public async Task<Role> GetRoleByIdAsync(
            RoleId id,
            CancellationToken cancellationToken = default
        )
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(
                r => r.Id == id,
                cancellationToken
            );

            if (role is null)
            {
                throw new DbNotFoundException(nameof(Role), id.Value.ToString());
            }

            return role;
        }

        public async Task<Role> GetRoleByNameAsync(
            string name,
            CancellationToken cancellationToken = default
        )
        {
            name = name.ToUpperInvariant();
            var role = await _dbContext.Roles.FirstOrDefaultAsync(
                r => EF.Property<string>(r, "_normalizedName") == name,
                cancellationToken
            );

            if (role is null)
            {
                throw new DbNotFoundException(nameof(Role), name);
            }

            return role;
        }
    }
}
