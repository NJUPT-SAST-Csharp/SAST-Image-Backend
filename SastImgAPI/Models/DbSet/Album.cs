using SastImgAPI.Models.Identity;
using SastImgAPI.Services;

namespace SastImgAPI.Models.DbSet
{
    public class Album
    {
        public long Id { get; set; } = CodeAccessor.GenerateSnowflakeId;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Uri? Cover { get; set; } = null;
        public User Author { get; set; } = null!;
        public long AuthorId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Accessibility Accessibility { get; set; } = Accessibility.Everyone;
        public ICollection<Image> Images { get; } = new List<Image>();
    }

    public enum Accessibility
    {
        Everyone,
        Auth,
        OnlyMe
    }
}
