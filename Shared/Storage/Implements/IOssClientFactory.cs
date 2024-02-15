using Aliyun.OSS;

namespace Shared.Storage.Implements
{
    internal interface IOssClientFactory
    {
        public OssClient GetOssClient();
    }
}
