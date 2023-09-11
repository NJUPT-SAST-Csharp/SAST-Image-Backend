using Microsoft.AspNetCore.Identity;
using SastImgAPI.Models.DbSet;
using SastImgAPI.Services;

namespace SastImgAPI.Models.Identity
{
    public class User : IdentityUser<long>
    {
        public override long Id { get; set; } = CodeAccessor.GenerateSnowflakeId;
        public string Nickname { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public Uri? Avatar { get; set; }
        public Uri? Header { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public ICollection<long> Likes { get; } = new List<long>();
        public ICollection<User> Following { get; } = new List<User>();
        public ICollection<User> Followers { get; } = new List<User>();
        public ICollection<Album> Albums { get; } = new List<Album>();
        public ICollection<Notification> Notifications { get; } = new List<Notification>();
    }
}
