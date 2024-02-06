using Aliyun.OSS;

namespace SastImg.Storage.Implements
{
    internal interface IOssClientFactory
    {
        public OssClient GetOssClient();
    }
}
