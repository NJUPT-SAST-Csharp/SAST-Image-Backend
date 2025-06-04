using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;
using SkiaSharp;
using Storage.Application.Model;

namespace Storage.Infrastructure.Models;

public sealed unsafe class ImageFile : IImageFile
{
    public const long DefaultMinSize = 1024 * 1024 * 0;
    public const long DefaultMaxSize = 1024 * 1024 * 256; // 256 MB

    private byte* _content;
    private readonly long _length;

    private ImageFile(byte* content, long length, ImageFileFormat format)
    {
        _content = content;
        _length = length;
        Format = format;
    }

    public Stream Stream
    {
        get
        {
            ObjectDisposedException.ThrowIf(_content is null, nameof(ImageFile));
            UnmanagedMemoryStream stream = new(_content, _length);
            return stream;
        }
    }

    public ImageFileFormat Format { get; }
    public long Length => _length;

    public static bool TryCreate(Stream stream, [NotNullWhen(true)] out ImageFile? imageFile)
    {
        if (stream.Length is > DefaultMaxSize or < DefaultMinSize)
        {
            imageFile = null;
            return false;
        }

        if (TryGetFormat(stream, out var format) is false)
        {
            imageFile = null;
            return false;
        }

        int size = (int)stream.Length;
        nint ptr = Marshal.AllocHGlobal(size);

        try
        {
            int readSize = ReadStreamToUnmanagedMemory(stream, ptr);
            if (readSize != size)
            {
                Marshal.FreeHGlobal(ptr);
                imageFile = null;
                return false;
            }
            imageFile = new((byte*)ptr.ToPointer(), stream.Length, format.Value);
            return true;
        }
        catch
        {
            Marshal.FreeHGlobal(ptr);
            imageFile = null;
            throw;
        }
    }

    public static bool TryGetFormat(Stream stream, [NotNullWhen(true)] out ImageFileFormat? format)
    {
        SKFrontBufferedManagedStream skStream = new(stream, SKCodec.MinBufferedBytesNeeded, false);

        var code = SKCodec.Create(skStream);

        // Reset stream position after reading
        if (stream.CanSeek)
        {
            stream.Position = 0;
            stream.Seek(0, SeekOrigin.Begin);
        }

        if (code is null)
        {
            format = null;
            return false;
        }

        format = (ImageFileFormat)code.EncodedFormat;
        return true;
    }

    public void Dispose()
    {
        if (_content == null)
            return;

        Marshal.FreeHGlobal((nint)_content);
        _content = null;
    }

    private static int ReadStreamToUnmanagedMemory(Stream stream, nint unmanagedPointer)
    {
        int totalBytesRead = 0;
        int bytesRead;
        const int bufferSize = 4 * 1024;

        Span<byte> buffer = stackalloc byte[bufferSize];

        while ((bytesRead = stream.Read(buffer)) > 0)
        {
            if (totalBytesRead + bytesRead > stream.Length)
                throw new InvalidOperationException("Read more bytes than expected.");

            byte* ptr = (byte*)unmanagedPointer + totalBytesRead;
            Unsafe.CopyBlock(ref Unsafe.AsRef<byte>(ptr), ref buffer[0], (uint)bytesRead);
            totalBytesRead += bytesRead;
        }

        return totalBytesRead;
    }
}

public static class HttpContextExtensions
{
    public static bool TryGetImageFile(
        this HttpContext context,
        [NotNullWhen(true)] out ImageFile? image
    )
    {
        image = null;

        var files = context.Request.Form.Files;

        if (files is not [var file])
            return false;

        if (ImageFile.TryCreate(file.OpenReadStream(), out image) is false)
            return false;

        context.Response.RegisterForDispose(image);
        return true;
    }
}
