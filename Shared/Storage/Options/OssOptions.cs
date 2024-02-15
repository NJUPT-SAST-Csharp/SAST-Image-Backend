namespace Shared.Storage.Options
{
    internal sealed class OssOptions
    {
        public const string Position = "Storage";
        public string Endpoint { get; init; }
        public string AccessKeyId { get; init; }
        public string AccessKeySecret { get; init; }
        public string ImageBucketName { get; init; }
        public string ThumbnailBucketName { get; init; }
    }
}
