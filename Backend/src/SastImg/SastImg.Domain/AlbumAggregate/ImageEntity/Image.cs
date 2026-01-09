using Primitives.Entity;
using Primitives.Utilities;
using SastImg.Domain.AlbumTagEntity;

namespace SastImg.Domain.AlbumAggregate.ImageEntity;

public sealed class Image : EntityBase<ImageId>
{
    private Image()
        : base(default) { }

    internal Image(ImageTitle title, ImageDescription description, ImageUrl url, ImageTagId[] tags)
        : base(new() { Value = SnowFlakeIdGenerator.NewId })
    {
        _title = title;
        _description = description;
        _url = url;
        _tags = tags;
    }

    #region Fields

    private readonly ImageTitle _title;

    private readonly ImageDescription _description;

    private readonly ImageUrl _url = null!;

    private readonly DateTime _uploadedAt = DateTime.UtcNow;

    private readonly ImageTagId[] _tags = [];

    private bool _isRemoved = false;

    #endregion

    #region Properties
    internal DateTime UploadedTime => _uploadedAt;

    internal ImageUrl Url => _url;

    internal bool IsRemoved => _isRemoved;

    #endregion

    #region Methods

    internal void Remove()
    {
        if (_isRemoved == false)
            _isRemoved = true;
    }

    internal void Restore()
    {
        if (_isRemoved == true)
            _isRemoved = false;
    }

    #endregion
}
