namespace Account.Entity.RoleEntity.Repositories
{
    public interface IRoleRespository
    {
        public Task<Role?> GetRoleByIdAsync(int id, CancellationToken cancellationToken = default);
        public Task<Role?> GetRoleByNameAsync(
            string name,
            CancellationToken cancellationToken = default
        );
    }
}
