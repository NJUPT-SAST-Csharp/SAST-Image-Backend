using Domain.Entity;

namespace Infrastructure.Shared.Storage;

internal abstract class StorageManagerBase(string basePath)
{
    private readonly string basePath = basePath;

    protected internal async Task StoreAsync<TId>(
        Stream file,
        TId id,
        string? filename = null,
        CancellationToken cancellationToken = default
    )
        where TId : ITypedId<TId, long>
    {
        string path = Path.Combine(basePath, id.GetRelativePath());
        EnsureDirectory(path);

        filename ??= id.Value.ToString();
        path = Path.Combine(path, filename);

        await using var stream = File.Create(path);

        await file.CopyToAsync(stream, cancellationToken);
    }

    protected internal Task DeleteAsync<TId>(TId id, CancellationToken cancellationToken = default)
        where TId : ITypedId<TId, long>
    {
        string path = Path.Combine(basePath, id.GetRelativePath());
        if (Directory.Exists(path))
            Directory.Delete(path, true);

        DeleteIfDirectoryEmpty(Directory.GetParent(path)!.FullName);
        return Task.CompletedTask;
    }

    protected internal Stream? OpenReadStream<TId>(TId id, string? mask = null)
        where TId : ITypedId<TId, long>
    {
        mask ??= id.Value.ToString();

        string path = Path.Combine(basePath, id.GetRelativePath());
        if (!Directory.Exists(path))
            return null;

        string[] files = Directory.GetFiles(path, mask, SearchOption.TopDirectoryOnly);
        if (files.Length <= 0)
            return null;

        var stream = File.OpenRead(files[0]);
        return stream;
    }

    private static void EnsureDirectory(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    private static void DeleteIfDirectoryEmpty(string path)
    {
        if (Directory.Exists(path) && Directory.GetFileSystemEntries(path).Length == 0)
            Directory.Delete(path);
    }
}
