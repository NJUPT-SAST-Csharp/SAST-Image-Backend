using Microsoft.Extensions.DependencyInjection;

namespace Storage.Clients
{
    internal sealed class StorageClientFactory(IServiceProvider provider) : IStorageClientFactory
    {
        private readonly IServiceProvider _provider = provider;

        public AvatarClient GetAvatarClient()
        {
            return _provider.GetRequiredService<AvatarClient>();
        }

        public HeaderClient GetHeaderClient()
        {
            return _provider.GetRequiredService<HeaderClient>();
        }

        public ImageClient GetImageClient()
        {
            return _provider.GetRequiredService<ImageClient>();
        }
    }
}
