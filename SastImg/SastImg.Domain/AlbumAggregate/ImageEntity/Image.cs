using Primitives.Entity;
using SastImg.Domain.AlbumAggregate.ImageEntity.Rules;
using Shared.Utilities;

namespace SastImg.Domain.AlbumAggregate.ImageEntity;

public sealed class Image : EntityBase<ImageId>
{
    private Image()
        : base(default) { }

    private Image(string title, Uri uri, string description, long[] tags)
        : base(new(SnowFlakeIdGenerator.NewId))
    {
        _title = title;
        _url = uri;
        _description = description;
        _tags = tags;
    }

    internal static Image CreateNewImage(string title, Uri uri, string description, long[] tags)
    {
        CheckRule(new ImageCannotOwnMoreThan5TagsRule(tags));
        return new Image(title, uri, description, tags);
    }

    #region Fields

    private string _title = string.Empty;

    private string _description = string.Empty;

    private readonly Uri _url;

    private readonly DateTime _uploadedAt = DateTime.UtcNow;

    private bool _isRemoved = false;

    private long[] _tags = [];

    #endregion

    #region Properties
    internal DateTime UploadedTime => _uploadedAt;

    internal Uri ImageUrl => _url;

    #endregion

    #region Methods


    internal void UpdateImageInfo(string title, string description, long[] tags)
    {
        CheckRule(new ImageCannotOwnMoreThan5TagsRule(tags));
        _title = title;
        _description = description;
        _tags = tags;
    }

    internal void Remove() => _isRemoved = true;

    internal void Restore() => _isRemoved = false;

    #endregion
}
