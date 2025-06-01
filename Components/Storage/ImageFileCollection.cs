using System.Collections;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace Storage;

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
            if (file.Length > ImageFile.MaxSize)
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
        if (TryCreate(context, out var collection))
        {
            return ValueTask.FromResult(collection);
        }

        return ValueTask.FromResult<ImageFileCollection?>(null);
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
