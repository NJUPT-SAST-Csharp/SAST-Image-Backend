using System.Buffers.Text;
using System.Text;

namespace Storage.Application.Model;

public readonly struct FileToken : IEquatable<FileToken>
{
    public const int ByteLength = 64;

    private readonly byte[] bytes;

    private FileToken(byte[] value)
    {
        bytes = value;
    }

    public string Value => ToString();
    public Guid ObjectName => new(bytes.AsSpan(16, 16));
    public string BucketName => Encoding.ASCII.GetString(bytes.AsSpan(0, 16)).TrimEnd('\0');

    public override string ToString() => Base64Url.EncodeToString(bytes);

    public bool Equals(FileToken other) => bytes.SequenceEqual(other.bytes);

    public override bool Equals(object? obj)
    {
        return obj is FileToken token && Equals(token);
    }

    public static bool operator ==(FileToken left, FileToken right) => left.Equals(right);

    public static bool operator !=(FileToken left, FileToken right) => !(left == right);

    public override int GetHashCode()
    {
        int hash = 17;
        foreach (byte b in bytes)
        {
            hash = hash * 31 + b.GetHashCode();
        }
        return hash;
    }
}
