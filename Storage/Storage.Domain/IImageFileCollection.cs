using System.Collections;

namespace Storage.Domain;

public interface IImageFileCollection
    : IReadOnlyList<IImageFile>,
        IEnumerable<IImageFile>,
        IEnumerable,
        IReadOnlyCollection<IImageFile> { }
