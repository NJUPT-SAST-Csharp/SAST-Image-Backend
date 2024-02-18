namespace Account.Application.AccountServices.Register.VerifyRegistrationCode
{
    public sealed class VerifyRegistrationCodeDto(bool isValidate)
    {
        public bool IsValidate { get; } = isValidate;
    }
}
