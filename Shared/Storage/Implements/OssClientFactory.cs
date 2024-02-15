using Aliyun.OSS;
using Microsoft.Extensions.Options;
using Shared.Storage.Options;

namespace Shared.Storage.Implements
{
    internal sealed class OssClientFactory : IOssClientFactory
    {
        private readonly OssClient _client;

        public OssClientFactory(IOptions<ImageOssOptions> options)
        {
            var value = options.Value;
            _client = new(value.Endpoint, value.AccessKeyId, value.AccessKeySecret);
        }

        public OssClient GetOssClient()
        {
            return _client;
        }
    }
}
