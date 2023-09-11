using Microsoft.EntityFrameworkCore;
using SastImgAPI.Services;

namespace SastImgAPI.Models.DbSet
{
    [Index(nameof(Name), IsUnique = true)]
    public class Tag
    {
        public long Id { get; set; } = CodeAccessor.GenerateSnowflakeId;
        public string Name { get; set; } = string.Empty;
        public ICollection<Image> Images { get; } = new List<Image>();
    }
}
