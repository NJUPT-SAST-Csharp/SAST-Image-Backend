using System.Collections;

namespace Storage.Entity;

public interface IImageFileCollection
    : IReadOnlyList<IImageFile>,
        IEnumerable<IImageFile>,
        IEnumerable,
        IReadOnlyCollection<IImageFile> { }
