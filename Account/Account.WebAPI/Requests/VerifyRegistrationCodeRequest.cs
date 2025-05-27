namespace Account.WebAPI.Requests;

public readonly struct VerifyRegistrationCodeRequest
{
    public readonly string Email { get; init; }
    public readonly int Code { get; init; }
}
