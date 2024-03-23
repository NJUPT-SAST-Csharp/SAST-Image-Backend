using Primitives.Entity;
using SastImg.Domain.AlbumAggregate.ImageEntity.Rules;
using SastImg.Domain.TagEntity;
using Utilities;

namespace SastImg.Domain.AlbumAggregate.ImageEntity;

public sealed class Image : EntityBase<ImageId>
{
    private Image()
        : base(default) { }

    internal Image(string title, string description, Uri url, Uri thumbnailUrl, TagId[] tags)
        : base(new(SnowFlakeIdGenerator.NewId))
    {
        CheckRule(new ImageOwnNotMoreThan5TagsRule(tags));

        _title = title;
        _description = description;
        _url = url;
        _thumbnailUrl = thumbnailUrl;
        _tags = tags;
    }

    #region Fields

    private readonly string _title = string.Empty;

    private readonly string _description = string.Empty;

    private readonly Uri _url;

    private readonly Uri _thumbnailUrl;

    private readonly DateTime _uploadedAt = DateTime.UtcNow;

    private readonly TagId[] _tags = [];

    private bool _isRemoved = false;

    #endregion

    #region Properties
    internal DateTime UploadedTime => _uploadedAt;

    internal Uri ImageUrl => _url;

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
