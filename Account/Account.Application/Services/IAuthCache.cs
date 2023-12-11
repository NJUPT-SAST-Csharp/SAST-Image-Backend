namespace Account.Application.Services
{
    public interface IAuthCache
    {
        public Task StoreCodeAsync(
            string purpose,
            string email,
            int code,
            TimeSpan expiry,
            CancellationToken cancellationToken = default
        );

        public Task StoreCodeAsync(
            string purpose,
            string email,
            int code,
            CancellationToken cancellationToken = default
        );

        public Task<bool> VerifyCodeAsync(
            string purpose,
            string email,
            int code,
            CancellationToken cancellationToken = default
        );

        public Task<bool> DeleteCodeAsync(
            string purpose,
            string email,
            CancellationToken cancellationToken = default
        );
    }
}
