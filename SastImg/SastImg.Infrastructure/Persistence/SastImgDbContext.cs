using Microsoft.EntityFrameworkCore;
using SastImg.Domain.Albums;
using SastImg.Domain.Albums.Images;
using SastImg.Domain.Categories;
using SastImg.Domain.Tags;

namespace SastImg.Infrastructure.Persistence
{
    public class SastImgDbContext : DbContext
    {
        public SastImgDbContext(DbContextOptions<SastImgDbContext> options)
            : base(options) { }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>().OwnsOne(a => a.Cover);
            modelBuilder.Entity<Image>().ToTable("images");
            base.OnModelCreating(modelBuilder);
        }
    }
}
