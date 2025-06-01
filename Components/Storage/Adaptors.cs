using SkiaSharp;

namespace Storage;

public static class Adaptors
{
    public static SKEncodedImageFormat ToSKFormat(this ImageFormat format)
    {
        return (SKEncodedImageFormat)format;
    }
}
