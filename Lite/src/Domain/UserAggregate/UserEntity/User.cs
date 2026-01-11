using Domain.Entity;
using Domain.UserAggregate.Commands;
using Domain.UserAggregate.Events;
using Domain.UserAggregate.Exceptions;
using Domain.UserAggregate.Services;

namespace Domain.UserAggregate.UserEntity;

public sealed class User : EntityBase<UserId>
{
    private User()
        : base(default) { }

    private Username _username;
    private Password _password = null!;
    private RefreshToken _refreshToken;
    private readonly Role[] _roles = [];

    private User(Username username, Password password)
        : base(UserId.GenerateNew())
    {
        _username = username;
        _password = password;
        _roles = [Role.User];
    }

    internal static async Task<JwtToken> RegisterAsync(
        RegisterCommand command,
        IUsernameUniquenessChecker usernameChecker,
        IRegistryCodeChecker codeChecker,
        IPasswordGenerator pwdGenerator,
        IJwtTokenGenerator jwtGenerator,
        IUserRepository repository,
        CancellationToken cancellationToken
    )
    {
        await Task.WhenAll(
            usernameChecker.CheckAsync(command.Username, cancellationToken),
            codeChecker.CheckAsync(command.Username, command.Code, cancellationToken)
        );

        var password = await pwdGenerator.GenerateAsync(command.Password, cancellationToken);

        User user = new(command.Username, password);

        var token = jwtGenerator.Generate(user.Id, user._username, new(user._roles));
        user._refreshToken = token.RefreshToken;
        await repository.AddAsync(user, cancellationToken);

        user.AddDomainEvent(new UserRegisteredEvent(user.Id, command.Username, command.Nickname));

        return token;
    }

    public async Task<JwtToken> LoginAsync(
        LoginCommand command,
        IPasswordValidator validator,
        IJwtTokenGenerator generator,
        CancellationToken cancellationToken
    )
    {
        await validator.ValidateAsync(_password, command.Password, cancellationToken);

        var token = generator.Generate(Id, _username, new(_roles));

        _refreshToken = token.RefreshToken;

        return token;
    }

    public async Task ResetPasswordAsync(
        ResetPasswordCommand command,
        IPasswordValidator validator,
        IPasswordGenerator generator,
        CancellationToken cancellationToken
    )
    {
        await validator.ValidateAsync(_password, command.OldPassword, cancellationToken);

        _password = await generator.GenerateAsync(command.NewPassword, cancellationToken);
    }

    public JwtToken RefreshToken(RefreshTokenCommand command, IJwtTokenGenerator generator)
    {
        if (
            _refreshToken == default
            || _refreshToken.IsExpired
            || _refreshToken != command.RefreshToken
        )
            throw new RefreshTokenInvalidException();

        var token = generator.Generate(Id, _username, new(_roles));
        _refreshToken = token.RefreshToken;
        return token;
    }

    public void ResetUsername(ResetUsernameCommand command)
    {
        _username = command.Username;

        AddDomainEvent(new UsernameResetEvent(Id, _username));
    }

    public void UpdateNickname(UpdateNicknameCommand command)
    {
        AddDomainEvent(new NicknameUpdatedEvent(Id, command.Nickname));
    }

    public void UpdateBiography(UpdateBiographyCommand command)
    {
        AddDomainEvent(new BiographyUpdatedEvent(Id, command.Biography));
    }

    public void UpdateAvatar(UpdateAvatarCommand command)
    {
        AddDomainEvent(new AvatarUpdatedEvent(Id, command.Avatar));
    }

    public void UpdateHeader(UpdateHeaderCommand command)
    {
        AddDomainEvent(new HeaderUpdatedEvent(Id, command.Header));
    }
}
