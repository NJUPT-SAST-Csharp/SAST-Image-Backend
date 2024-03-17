using Primitives.Entity;
using SastImg.Domain.AlbumAggregate.ImageEntity.Rules;
using SastImg.Domain.TagEntity;
using Utilities;

namespace SastImg.Domain.AlbumAggregate.ImageEntity;

public sealed class Image : EntityBase<ImageId>
{
    private Image()
        : base(default) { }

    private Image(string title, Uri uri, string description, TagId[] tags)
        : base(new(SnowFlakeIdGenerator.NewId))
    {
        _title = title;
        _url = uri;
        _description = description;
        _tags = tags;
    }

    internal static Image CreateNewImage(string title, Uri uri, string description, TagId[] tags)
    {
        CheckRule(new ImageOwnNotMoreThan5TagsRule(tags));
        Image image = new(title, uri, description, tags);
        return image;
    }

    #region Fields

    private readonly string _title = string.Empty;

    private readonly string _description = string.Empty;

    private readonly Uri _url;

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
