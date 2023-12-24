using SastImg.Domain.AlbumAggregate.Rules;
using Shared.Primitives;
using Shared.Utilities;

namespace SastImg.Domain.AlbumAggregate
{
    public sealed class Image : Entity<long>
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        private Image()
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
            : base(0) { }

        private Image(string title, Uri uri, string description)
            : base(SnowFlakeIdGenerator.NewId)
        {
            _title = title;
            _url = uri;
            _description = description;
        }

        internal static Image CreateNewImage(
            string title,
            Uri uri,
            string description,
            IEnumerable<long> tags
        )
        {
            CheckRule(new ImageCannotOwnMoreThan5TagsRule(tags));
            return new Image(title, uri, description);
        }

        #region Fields

        private string _title = string.Empty;

        private string _description = string.Empty;

        private readonly Uri _url;

        private readonly DateTime _uploadedAt = DateTime.Now;

        private bool _isRemoved = false;

        private bool _isNsfw = false;

        private List<long> _tags = [];

        #endregion

        #region Properties
        internal DateTime UploadedTime => _uploadedAt;

        internal Uri ImageUrl => _url;

        #endregion

        #region Methods


        internal void UpdateImageInfo(
            string title,
            string description,
            bool isNsfw,
            IEnumerable<long> tags
        )
        {
            CheckRule(new ImageCannotOwnMoreThan5TagsRule(tags));
            _isNsfw = isNsfw;
            _title = title;
            _description = description;
            _tags = tags.ToList();
        }

        internal void Remove() => _isRemoved = true;

        internal void Restore() => _isRemoved = false;

        internal void SetNsfw(bool isNsfw) => _isNsfw = isNsfw;

        #endregion
    }
}
