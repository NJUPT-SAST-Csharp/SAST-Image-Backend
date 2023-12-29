namespace SastImg.Application.SeedWorks
{
    public interface ICache<T>
    {
        public Task<T?> GetCachingAsync(string key, CancellationToken cancellationToken = default);

        public Task DeleteCachingAsync(string key, CancellationToken cancellationToken = default);

        public Task ResetCachingAsync(
            string key,
            T? value,
            CancellationToken cancellationToken = default
        );
    }
}
