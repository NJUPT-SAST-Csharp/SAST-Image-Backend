using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using SkiaSharp;
using Storage.Application.Model;

namespace Storage.Infrastructure.Models;

public static class Adaptors
{
    public static SKEncodedImageFormat ToSKFormat(this ImageFileFormat format)
    {
        return (SKEncodedImageFormat)format;
    }

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

        string filename = file.FileName.Split('.')[0] + "." + image.Format;
        image.MetaData.Add(nameof(IFormFile.FileName), filename);
        context.Response.RegisterForDispose(image);
        return true;
    }
}
