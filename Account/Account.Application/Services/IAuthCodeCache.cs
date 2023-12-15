namespace Account.Application.Services
{
    public interface IAuthCodeCache
    {
        public Task StoreCodeAsync(
            CodeCaheKey purpose,
            string email,
            int code,
            TimeSpan expiry,
            CancellationToken cancellationToken = default
        );

        public Task StoreCodeAsync(
            CodeCaheKey purpose,
            string email,
            int code,
            CancellationToken cancellationToken = default
        );

        public Task<bool> VerifyCodeAsync(
            CodeCaheKey purpose,
            string email,
            int code,
            CancellationToken cancellationToken = default
        );

        public Task<bool> DeleteCodeAsync(
            CodeCaheKey purpose,
            string email,
            CancellationToken cancellationToken = default
        );
    }
}
