namespace Account.WebAPI.Requests;

public readonly struct SendForgetCodeRequest(string email)
{
    public string Email { get; init; } = email;
}
