using Account.Entity.UserEntity;

namespace Account.Entity.RoleEntity
{
    public sealed class Role(string name)
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = name.ToUpperInvariant();

        public ICollection<User> Users { get; } = [];
    }
}
