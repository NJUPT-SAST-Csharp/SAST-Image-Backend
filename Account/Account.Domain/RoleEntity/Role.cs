using Primitives.Entity;

namespace Account.Domain.RoleEntity
{
    public sealed class Role : EntityBase<RoleId>
    {
        private Role()
            : base(default) { }

        private Role(string name)
            : base(new(0))
        {
            _name = name.ToUpperInvariant();
        }

        public static Role CreateNewRole(string name)
        {
            var role = new Role(name);
            // TODO: Add domain event
            return role;
        }

        #region Fields

        private readonly string _name;

        #endregion

        #region Properties

        public string Name => _name;

        #endregion
    }
}
