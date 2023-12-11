namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode
{
    public sealed class VerifyForgetCodeDto(string username, int code)
    {
        public string Username { get; } = username;
        public int Code { get; } = code;
    }
}
