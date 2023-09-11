using SastImgAPI.Services;

namespace SastImgAPI.Models.DbSet
{
    public class Notification
    {
        public long Id { get; set; } = CodeAccessor.GenerateSnowflakeId;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
    }
}
