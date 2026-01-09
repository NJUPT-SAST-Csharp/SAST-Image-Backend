namespace Storage.Application.Model;

public interface IFileToken : IEquatable<IFileToken>
{
    public string Value { get; }
    public string ObjectName { get; }
    public string BucketName { get; }

    public DateTime ExpireAt { get; }
}
