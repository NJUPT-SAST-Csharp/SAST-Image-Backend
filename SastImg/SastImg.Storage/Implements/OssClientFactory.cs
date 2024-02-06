using Aliyun.OSS;
using Microsoft.Extensions.Options;
using SastImg.Storage.Options;

namespace SastImg.Storage.Implements
{
    internal sealed class OssClientFactory : IOssClientFactory
    {
        private readonly OssClient _client;

        public OssClientFactory(IOptions<OssOptions> options)
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
