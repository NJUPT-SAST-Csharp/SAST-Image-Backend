using SkiaSharp;
using Storage.Application.Model;

namespace Storage.Infrastructure.Models;

public static class Adaptors
{
    public static SKEncodedImageFormat ToSKFormat(this ImageFileFormat format)
    {
        return (SKEncodedImageFormat)format;
    }
}
