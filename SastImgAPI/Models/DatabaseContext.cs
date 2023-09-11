using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SastImgAPI.Models.DbSet;
using SastImgAPI.Models.Identity;
using Image = SastImgAPI.Models.DbSet.Image;
using System.Xml;
using System.Reflection.Metadata;

namespace SastImgAPI.Models
{
    public class DatabaseContext : IdentityDbContext<User, Role, long>, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }

        public DbSet<Image> Images { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var urlConverter = modelBuilder
                .Entity<Image>()
                .Property(image => image.Url)
                .HasConversion<string>();
        }
    }

    interface IDatabaseContext { }
}
