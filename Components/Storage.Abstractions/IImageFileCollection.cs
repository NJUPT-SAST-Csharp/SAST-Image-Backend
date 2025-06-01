using System.Collections;

namespace Storage;

public interface IImageFileCollection
    : IReadOnlyList<IImageFile>,
        IEnumerable<IImageFile>,
        IEnumerable,
        IReadOnlyCollection<IImageFile> { }
