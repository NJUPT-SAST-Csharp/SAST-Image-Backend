namespace Account.WebAPI.Requests;

public readonly struct SendRegistrationCodeRequest
{
    public readonly string Email { get; init; }
}
