using Account.Entity.RoleEntity;
using Account.Entity.RoleEntity.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.Persistence
{
    public sealed class RoleRepository(AccountDbContext context) : IRoleRespository
    {
        private readonly AccountDbContext _dbContext = context;

        public Task<Role?> GetRoleByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public Task<Role?> GetRoleByNameAsync(
            string name,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext.Roles.FirstOrDefaultAsync(
                r => r.NormalizedName == name.ToUpperInvariant(),
                cancellationToken
            );
        }
    }
}
