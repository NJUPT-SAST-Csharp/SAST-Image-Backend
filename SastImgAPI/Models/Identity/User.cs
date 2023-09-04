using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SastImgAPI.Models.DbSet;
using System.ComponentModel.DataAnnotations;

namespace SastImgAPI.Models.Identity
{
    public class User : IdentityUser<int>
    {
        public string Nickname { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Header { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public TimeZoneInfo TimeZone { get; set; } =
            TimeZoneInfo.FindSystemTimeZoneById("Asia/Shanghai");
        public string Language { get; set; } = "zh/cn";
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public ICollection<int> Likes { get; } = new List<int>();
        public ICollection<User> Following { get; } = new List<User>();
        public ICollection<User> Followers { get; } = new List<User>();
        public ICollection<Album> Albums { get; } = new List<Album>();
        public ICollection<Notification> Notifications { get; } = new List<Notification>();
    }
}
