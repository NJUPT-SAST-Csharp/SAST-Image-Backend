using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SastImgAPI.Models.DbSet;
using SastImgAPI.Models.Identity;
using Image = SastImgAPI.Models.DbSet.Image;
using System.Xml;
using System.Reflection.Metadata;

namespace SastImgAPI.Models
{
    public class TimeZoneInfoToStringConverter : ValueConverter<TimeZoneInfo, string>
    {
        public TimeZoneInfoToStringConverter(ConverterMappingHints? mappingHints = null)
            : base(tz => tz.Id, str => TimeZoneInfo.FindSystemTimeZoneById(str), mappingHints) { }
    }

    public class DatabaseContext : IdentityDbContext<User, Role, int>
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
            var uriConverter = new UriToStringConverter();
            var timezoneConverter = new TimeZoneInfoToStringConverter();

            modelBuilder
                .Entity<Album>()
                .HasOne(e => e.Cover)
                .WithOne()
                .HasForeignKey<Album>(e => e.CoverId)
                .IsRequired(false);

            modelBuilder
                .Entity<User>()
                .Property(user => user.TimeZone)
                .HasConversion(timezoneConverter);
        }
    }
}
