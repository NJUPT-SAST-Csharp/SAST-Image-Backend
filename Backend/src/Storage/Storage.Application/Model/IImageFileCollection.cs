using System.Collections;

namespace Storage.Application.Model;

public interface IImageFileCollection
    : IReadOnlyList<IImageFile>,
        IEnumerable<IImageFile>,
        IEnumerable,
        IReadOnlyCollection<IImageFile> { }
