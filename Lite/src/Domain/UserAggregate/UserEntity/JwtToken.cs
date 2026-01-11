namespace Domain.UserAggregate.UserEntity;

public readonly record struct JwtToken(
    string AccessToken,
    RefreshToken RefreshToken,
    long ExpireIn
);
