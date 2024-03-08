namespace Account.WebAPI.Requests
{
    public readonly struct VerifyForgetCodeRequest
    {
        public readonly string Email { get; init; }
        public readonly int Code { get; init; }
    }
}
