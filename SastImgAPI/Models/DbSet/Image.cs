using SastImgAPI.Models.Identity;
using SastImgAPI.Services;

namespace SastImgAPI.Models.DbSet
{
    public class Image
    {
        public long Id { get; set; } = CodeAccessor.GenerateSnowflakeId;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Tag> Tags { get; } = new List<Tag>();
        public Category Category { get; set; } = null!;
        public long CategoryId { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Album Album { get; set; } = null!;
        public long AlbumId { get; set; }
        public User Author { get; set; } = null!;
        public long AuthorId { get; set; }
        public ICollection<long> LikedBy { get; } = new List<long>();
        public int Views { get; set; } = 0;
        public bool IsExifEnabled { get; set; } = false;
        public Uri Url { get; set; } = null!;
    }
}
