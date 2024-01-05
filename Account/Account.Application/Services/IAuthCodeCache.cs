namespace Account.Application.Services
{
    public interface IAuthCodeCache
    {
        public Task StoreCodeAsync(
            CodeCacheKey purpose,
            string email,
            int code,
            TimeSpan expiry,
            CancellationToken cancellationToken = default
        );

        public Task StoreCodeAsync(
            CodeCacheKey purpose,
            string email,
            int code,
            CancellationToken cancellationToken = default
        );

        public Task<bool> VerifyCodeAsync(
            CodeCacheKey purpose,
            string email,
            int code,
            CancellationToken cancellationToken = default
        );

        public Task<bool> DeleteCodeAsync(
            CodeCacheKey purpose,
            string email,
            CancellationToken cancellationToken = default
        );
    }
}
