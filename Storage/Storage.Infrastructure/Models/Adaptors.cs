using SkiaSharp;
using Storage.Domain;

namespace Storage.Infrastructure.Models;

public static class Adaptors
{
    public static SKEncodedImageFormat ToSKFormat(this ImageFormat format)
    {
        return (SKEncodedImageFormat)format;
    }
}
