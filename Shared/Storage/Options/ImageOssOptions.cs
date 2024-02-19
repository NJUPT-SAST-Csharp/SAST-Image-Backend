namespace Shared.Storage.Options
{
    public sealed class ImageOssOptions
    {
        public const string Position = "Storage";
        public string Endpoint { get; init; }
        public string AccessKeyId { get; init; }
        public string AccessKeySecret { get; init; }
        public string BucketName { get; init; }
    }
}
