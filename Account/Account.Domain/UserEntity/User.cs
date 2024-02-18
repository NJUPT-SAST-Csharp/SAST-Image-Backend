using Account.Domain.RoleEntity;
using Account.Domain.UserEntity.Events;
using Account.Domain.UserEntity.Rules;
using Primitives.Entity;
using Shared.Primitives;
using Utilities;

namespace Account.Domain.UserEntity
{
    public sealed class User : EntityBase<UserId>, IAggregateRoot<User>
    {
        private User()
            : base(default) { }

        private User(string username, string password, string email)
            : base(new(SnowFlakeIdGenerator.NewId))
        {
            _username = username;
            _email = email.ToUpperInvariant();
            _passwordSalt = Argon2Hasher.RandomSalt;
            _passwordHash = Argon2Hasher.Hash(password, _passwordSalt);
            _registerAt = DateTime.UtcNow;
        }

        public static User CreateNewUser(string username, string password, string email)
        {
            var user = new User(username, password, email);
            user.AddDomainEvent(new UserCreatedEvent(user));
            return user;
        }

        #region Fields

        private readonly List<Role> _roles = [];

        private readonly string _username;
        private readonly string _email;
        private byte[] _passwordHash;
        private byte[] _passwordSalt;

        private readonly DateTime _registerAt;
        private DateTime _loginAt;

        private Profile _profile;

        #endregion

        #region Properties

        public string Username => _username;

        public IReadOnlyCollection<Role> Roles => _roles;

        #endregion

        #region Methods

        public void AddRole(Role role) => _roles.Add(role);

        public async Task ResetPasswordAsync(string newPassword)
        {
            Argon2Hasher.RegenerateSalt(_passwordSalt);
            _passwordHash = await Argon2Hasher.HashAsync(newPassword, _passwordSalt);
        }

        public async Task LoginAsync(string password)
        {
            await CheckRuleAsync(new LoginRule(password, _passwordHash, _passwordSalt));
            _loginAt = DateTime.UtcNow;
        }

        public void UpdateProfile(string nickname, string biography, Uri? website)
        {
            _profile.UpdateProfile(nickname, biography, website);
        }

        public void UpdateAvatar(Uri? avatar) => _profile.UpdateAvatar(avatar);

        public void UpdateHeader(Uri? header) => _profile.UpdateHeader(header);

        #endregion
    }
}
