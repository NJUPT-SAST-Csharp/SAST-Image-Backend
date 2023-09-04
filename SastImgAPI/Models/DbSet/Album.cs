using Microsoft.EntityFrameworkCore;
using SastImgAPI.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace SastImgAPI.Models.DbSet
{
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Image Cover { get; set; } = null!;
        public int CoverId { get; set; }
        public User Author { get; set; } = null!;
        public int AuthorId { get; set; }
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
