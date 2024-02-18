using Account.Domain.UserEntity.Events;
using Account.Domain.UserEntity.Rules;
using Account.Domain.UserEntity.ValueObjects;
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
            _password = Password.NewPassword(password);
            _registerAt = DateTime.UtcNow;
            _loginAt = DateTime.UtcNow;
            _roles = [Role.USER];
        }

        public static User CreateNewUser(string username, string password, string email)
        {
            CheckRule(new UsernameValidRule(username));
            CheckRule(new PasswordValidRule(password));
            CheckRule(new EmailValidRule(email));

            var user = new User(username, password, email);
            user.AddDomainEvent(new UserCreatedEvent(user));
            return user;
        }

        #region Fields

        private readonly string _username;
        private readonly string _email;
        private Uri? _avatar = null;
        private Uri? _header = null;
        private readonly DateTime _registerAt;
        private DateTime _loginAt;

        private Profile _profile;
        private Password _password;

        private Role[] _roles;

        #endregion

        #region Properties

        public string Username => _username;

        public IReadOnlyCollection<Role> Roles => _roles;

        #endregion

        #region Methods

        public void UpdateAuthorizations(params Role[] roles)
        {
            _roles = roles;
        }

        public void ResetPassword(string newPassword)
        {
            CheckRule(new PasswordValidRule(newPassword));

            _password = Password.NewPassword(newPassword);
        }

        public async Task<bool> LoginAsync(string password)
        {
            CheckRule(new PasswordValidRule(password));

            if (await _password.ValidateAsync(password) is false)
            {
                return false;
            }

            _loginAt = DateTime.UtcNow;
            return true;
        }

        public void UpdateProfile(
            string nickname,
            string biography,
            DateOnly birthday,
            Uri? website
        )
        {
            CheckRule(new NicknameLengthRule(nickname));
            CheckRule(new BiographyValidRule(biography));

            _profile = new(nickname, biography, birthday, website);
        }

        public void UpdateAvatar(Uri? avatar) => _avatar = avatar;

        public void UpdateHeader(Uri? header) => _header = header;

        #endregion
    }
}
