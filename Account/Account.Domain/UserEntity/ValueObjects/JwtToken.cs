namespace Account.Domain.UserEntity.ValueObjects;

public readonly record struct JwtToken(string AccessToken, long ExpireIn);
