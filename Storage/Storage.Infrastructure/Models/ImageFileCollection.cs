using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Storage.Domain;

namespace Storage.Infrastructure.Models;

public sealed class ImageFileCollection : IImageFileCollection
{
    private readonly List<IImageFile> files = [];

    private ImageFileCollection(IEnumerable<IImageFile> files)
    {
        this.files.AddRange(files);
    }

    private ImageFileCollection() { }

    public static bool TryCreate(HttpContext context, out ImageFileCollection? imageCollection)
    {
        var files = context.Request.Form.Files;
        if (files.Count <= 0)
        {
            imageCollection = new();
            return true;
        }

        List<ImageFile> images = [];
        foreach (var file in files)
        {
            if (file.Length is > ImageFile.DefaultMaxSize or < ImageFile.DefaultMinSize)
            {
                imageCollection = null;
                return false;
            }
            if (ImageFile.TryCreate(file.OpenReadStream(), out var image) is false)
            {
                imageCollection = null;
                return false;
            }

            context.Response.RegisterForDispose(image);
            images.Add(image);
        }

        imageCollection = new(images);
        return true;
    }

    public static ValueTask<ImageFileCollection?> BindAsync(HttpContext context, ParameterInfo info)
    {
        var attributes = info.GetCustomAttributes<ValidationAttribute>();
        foreach (var attribute in attributes)
            if (attribute.IsValid(context.Request.Form.Files) is false)
                return ValueTask.FromResult<ImageFileCollection?>(null);

        if (TryCreate(context, out var collection) is false)
        {
            return ValueTask.FromResult<ImageFileCollection?>(null);
        }

        return ValueTask.FromResult(collection);
    }

    public IImageFile this[int index] => files[index];

    public int Count => files.Count;

    public IEnumerator<IImageFile> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
