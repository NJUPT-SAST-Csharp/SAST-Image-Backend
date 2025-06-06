using System.Buffers.Text;
using System.Text;
using Storage.Application.Model;

namespace Storage.Infrastructure.Models;

public sealed class FileToken : IFileToken, IEquatable<FileToken>
{
    private readonly byte[] bytes;

    internal FileToken(string base64String)
    {
        bytes = Base64Url.DecodeFromChars(base64String);
    }

    public string Value => Base64Url.EncodeToString(bytes);
    public DateTime ExpireAt => DateTime.FromBinary(BitConverter.ToInt64(bytes.AsSpan()[^40..^32]));
    public string ObjectName => new Guid(bytes.AsSpan()[^56..^40]).ToString();
    public string BucketName => Encoding.ASCII.GetString(bytes.AsSpan()[..^56]).TrimEnd('\0');

    public override string ToString() => Value;

    public bool Equals(IFileToken? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Value == other.Value;
    }

    public bool Equals(FileToken? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj) => Equals(obj as FileToken);

    public override int GetHashCode() => Value.GetHashCode();
}
