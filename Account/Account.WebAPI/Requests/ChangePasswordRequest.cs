namespace Account.WebAPI.Requests;

public readonly struct ChangePasswordRequest
{
    public readonly string NewPassword { get; init; }
}
