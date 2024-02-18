namespace Account.Domain.RoleEntity.Services
{
    public interface IRoleRespository
    {
        public Task<RoleId> AddNewRoleAsync(
            Role role,
            CancellationToken cancellationToken = default
        );

        public Task<Role> GetRoleByIdAsync(
            RoleId roleId,
            CancellationToken cancellationToken = default
        );

        public Task<Role> GetRoleByNameAsync(
            string roleName,
            CancellationToken cancellationToken = default
        );
    }
}
