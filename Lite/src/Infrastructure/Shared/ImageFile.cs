using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Domain.Shared;

namespace Infrastructure.Shared;

public unsafe struct ImageFile : IDisposable, IImageFile
{
    private byte* _content;
    private readonly long _length;

    private ImageFile(byte* content, long length)
    {
        _content = content;
        _length = length;
    }

    public readonly Stream Stream
    {
        get
        {
            if (_content == null)
                throw new ObjectDisposedException(nameof(ImageFile));

            UnmanagedMemoryStream stream = new(_content, _length);
            return stream;
        }
    }

    public static ImageFile Create(Stream stream)
    {
        if (stream.Length > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(stream));

        int size = (int)stream.Length;
        nint ptr = Marshal.AllocHGlobal(size);
        int readSize = ReadStreamToUnmanagedMemory(stream, ptr);

        if (readSize != size)
            throw new InvalidOperationException(
                "The number of bytes read from the stream does not match the stream length."
            );

        stream.Dispose();
        return new((byte*)ptr.ToPointer(), stream.Length);
    }

    public void Dispose()
    {
        if (_content == null)
            return;

        Marshal.FreeHGlobal((nint)_content);
        _content = null;
    }

    private static int ReadStreamToUnmanagedMemory(Stream stream, IntPtr unmanagedPointer)
    {
        int totalBytesRead = 0;
        int bytesRead;
        const int bufferSize = 4 * 1024;

        byte[] pool = ArrayPool<byte>.Shared.Rent(bufferSize);

        while ((bytesRead = stream.Read(pool, 0, bufferSize)) > 0)
        {
            byte* ptr = (byte*)unmanagedPointer + totalBytesRead;
            Unsafe.CopyBlock(ref Unsafe.AsRef<byte>(ptr), ref pool[0], (uint)bytesRead);
            totalBytesRead += bytesRead;
        }

        ArrayPool<byte>.Shared.Return(pool);

        return totalBytesRead;
    }
}
