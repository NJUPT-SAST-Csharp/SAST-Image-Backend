using SastImgAPI.Models.Identity;

namespace SastImgAPI.Models.DbSet
{
    public class Image
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<Tag> Tags { get; } = new List<Tag>();

        public Category Category { get; set; } = null!;
        public int CategoryId { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Album From { get; set; } = null!;
        public int FromId { get; set; }
        public User Author { get; set; } = null!;
        public int AuthorId { get; set; }
        public ICollection<int> Likes { get; } = new List<int>();
        public int Views { get; set; } = 0;
        public bool IsExifEnabled { get; set; } = false;
        public string Url { get; set; } = string.Empty;
    }
}
