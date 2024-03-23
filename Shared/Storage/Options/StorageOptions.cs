using System.Text;

namespace Storage.Options
{
    public sealed class StorageOptions
    {
        public const string Position = "Storage";

        private readonly StringBuilder _builder = new(128);

        public string Endpoint { get; set; }
        public string AccessKeyId { get; set; }
        public string AccessKeySecret { get; set; }
        public string BucketName { get; set; }

        public string GetUrl(string key)
        {
            _builder.Clear();

            int headerIndex = Endpoint.IndexOf('/') + 2;

            return _builder
                .Append(Endpoint, 0, headerIndex)
                .Append(BucketName)
                .Append('.')
                .Append(Endpoint, headerIndex, Endpoint.Length - headerIndex)
                .Append('/')
                .Append(key)
                .ToString();
        }
    }
}
