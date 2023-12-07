namespace Account.Application.Services
{
    public interface IAuthCache
    {
        public Task StoreCodeAsync(
            string email,
            string code,
            TimeSpan expiry,
            CancellationToken cancellationToken = default
        );

        public Task StoreCodeAsync(
            string email,
            string code,
            CancellationToken cancellationToken = default
        );

        public Task<bool> VerifyCodeAsync(
            string email,
            string code,
            CancellationToken cancellationToken = default
        );

        public Task<bool> DeleteCodeAsync(
            string email,
            CancellationToken cancellationToken = default
        );
    }
}
