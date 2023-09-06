using Microsoft.EntityFrameworkCore;

namespace SastImgAPI.Models.DbSet
{
    [Index(nameof(Name), IsUnique = true)]
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Image> Images { get; } = new List<Image>();
    }
}
