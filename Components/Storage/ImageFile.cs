using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;
using SkiaSharp;

namespace Storage;

public sealed unsafe class ImageFile : IDisposable, IImageFile
{
    public const int MinSize = 1024 * 1024 * 0;
    public const int MaxSize = 1024 * 1024 * 256; // 256 MB

    private byte* _content;
    private readonly long _length;

    private ImageFile(byte* content, long length, ImageFormat format)
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

    public ImageFormat Format { get; }
    public long Length => _length;

    public static bool TryCreate(Stream stream, [NotNullWhen(true)] out ImageFile? imageFile)
    {
        if (stream.Length > MaxSize)
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

    public static ValueTask<ImageFile?> BindAsync(HttpContext context, ParameterInfo info)
    {
        var files = context.Request.Form.Files;

        if (files.Count <= 0)
            return ValueTask.FromResult(null as ImageFile);

        var file = files[0];

        var fileSizeAttri = info.GetCustomAttribute<FileSizeAttribute>();
        if (fileSizeAttri is not null && fileSizeAttri.IsValid(file) is false)
            return ValueTask.FromResult(null as ImageFile);

        if (TryCreate(file.OpenReadStream(), out var imageFile) is false)
            return ValueTask.FromResult(null as ImageFile);

        context.Response.RegisterForDispose(imageFile);

        return ValueTask.FromResult<ImageFile?>(imageFile);
    }

    public void Dispose()
    {
        if (_content == null)
            return;

        Marshal.FreeHGlobal((nint)_content);
        _content = null;
    }

    private static bool TryGetFormat(Stream stream, [NotNullWhen(true)] out ImageFormat? format)
    {
        using SKFrontBufferedManagedStream skStream = new(
            stream,
            SKCodec.MinBufferedBytesNeeded,
            true
        );

        using var code = SKCodec.Create(skStream);

        if (code is null)
        {
            format = null;
            return false;
        }

        format = (ImageFormat)code.EncodedFormat;
        return true;
    }

    private static int ReadStreamToUnmanagedMemory(Stream stream, IntPtr unmanagedPointer)
    {
        int totalBytesRead = 0;
        int bytesRead;
        const int bufferSize = 4 * 1024;

        byte[] pool = ArrayPool<byte>.Shared.Rent(bufferSize);

        try
        {
            while ((bytesRead = stream.Read(pool, 0, bufferSize)) > 0)
            {
                if (totalBytesRead + bytesRead > stream.Length)
                    throw new InvalidOperationException("Read more bytes than expected.");

                byte* ptr = (byte*)unmanagedPointer + totalBytesRead;
                Unsafe.CopyBlock(ref Unsafe.AsRef<byte>(ptr), ref pool[0], (uint)bytesRead);
                totalBytesRead += bytesRead;
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(pool);
        }

        return totalBytesRead;
    }
}
