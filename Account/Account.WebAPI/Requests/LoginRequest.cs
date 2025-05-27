namespace Account.WebAPI.Requests;

public readonly struct LoginRequest
{
    public readonly string Username { get; init; }
    public readonly string Password { get; init; }
}
