using Account.Domain.UserEntity.Events;
using Account.Domain.UserEntity.ValueObjects;
using Identity;
using Primitives.Entity;

namespace Account.Domain.UserEntity;

public sealed class User : EntityBase<UserId>, IAggregateRoot<User>
{
    private User()
        : base(default) { }

    private User(string username, string password, string email)
        : base(UserId.GenerateNew())
    {
        _username = username;
        _email = email.ToUpperInvariant();
        _password = Password.NewPassword(password);
        _registerAt = DateTime.UtcNow;
        _loginAt = DateTime.UtcNow;
        _roles = [Role.USER];
        _profile = Profile.Default;
    }

    public static User CreateNewUser(string username, string password, string email)
    {
        var user = new User(username, password, email);
        user.AddDomainEvent(new UserCreatedEvent(user));
        return user;
    }

    #region Fields

    private readonly string _username = null!;
    private readonly string _email = null!;
    private Uri? _avatar = null;
    private Uri? _header = null;
    private readonly DateTime _registerAt;
    private DateTime _loginAt;

    private Profile _profile = null!;
    private Password _password = null!;

    private Role[] _roles = [];

    #endregion

    #region Properties

    public string Username => _username;

    public IReadOnlyCollection<Role> UserRoles => _roles;

    #endregion

    #region Methods

    public void UpdateAuthorizations(params Role[] roles)
    {
        _roles = roles;
    }

    public void ResetPassword(string newPassword)
    {
        _password = Password.NewPassword(newPassword);
    }

    public async Task<bool> LoginAsync(string password)
    {
        if (await _password.ValidateAsync(password) is false)
        {
            return false;
        }

        _loginAt = DateTime.UtcNow;
        return true;
    }

    public void UpdateProfile(string nickname, string biography, DateOnly? birthday, Uri? website)
    {
        _profile = new(nickname, biography, birthday, website);
    }

    public void UpdateAvatar(Uri? avatar) => _avatar = avatar;

    public void UpdateHeader(Uri? header) => _header = header;

    #endregion
}
