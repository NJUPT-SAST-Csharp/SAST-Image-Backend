using Account.Domain.UserEntity.Commands;
using Account.Domain.UserEntity.Exceptions;
using Account.Domain.UserEntity.Services;
using Account.Domain.UserEntity.ValueObjects;
using Identity;
using Primitives.Entity;

namespace Account.Domain.UserEntity;

public sealed class User : EntityBase<UserId>, IAggregateRoot<User>
{
    private User()
        : base(default) { }

    public static async ValueTask<User> RegisterAsync(
        RegisterCommand command,
        IPasswordGenerator passwordGenerator,
        IUsernameUniquenessChecker checker
    )
    {
        if (await checker.ExistsAsync(command.Username))
            throw new UsernameDuplicatedDomainException(command.Username);

        User user = new()
        {
            Password = await passwordGenerator.GenerateAsync(command.Password),
            Username = command.Username,
            Roles = [Role.USER],
        };

        return user;
    }

    #region Properties

    public Username Username { get; private set; }
    public Role[] Roles { get; private set; } = [];
    internal ImageToken? Avatar { get; private set; } = null;
    internal ImageToken? Header { get; private set; } = null;
    internal Password Password { get; private set; } = null!;

    #endregion

    #region Methods

    public void Authorize(AuthorizeCommand command)
    {
        NoPermissionDomainException.ThrowIf(command.Requester.IsAdmin is false);

        Roles = command.Roles;
    }

    public async ValueTask ResetPasswordAsync(
        ChangePasswordCommand command,
        IPasswordGenerator generator
    )
    {
        NoPermissionDomainException.ThrowIf(
            command.Requester.Id != Id || command.Requester.IsAdmin is false
        );

        var password = await generator.GenerateAsync(command.NewPassword);
        Password = password;
    }

    public async ValueTask<JwtToken> LoginAsync(
        LoginCommand command,
        IPasswordValidator validator,
        IJwtTokenGenerator generator
    )
    {
        LoginFailDomainException.ThrowIf(await validator.ValidateAsync(command.Password, Password));

        return generator.Issue(this);
    }

    public void UpdateUsername(UpdateUsernameCommand command)
    {
        NoPermissionDomainException.ThrowIf(
            command.Requester.Id != Id || command.Requester.IsAdmin is false
        );

        Username = command.Username;
    }

    #endregion
}
