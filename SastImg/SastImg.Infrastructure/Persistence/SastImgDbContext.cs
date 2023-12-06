using Microsoft.EntityFrameworkCore;
using SastImg.Domain.Albums;
using SastImg.Domain.Categories;
using SastImg.Domain.Tags;

namespace SastImg.Infrastructure.Persistence
{
    public sealed class SastImgDbContext : DbContext
    {
        public SastImgDbContext(DbContextOptions<SastImgDbContext> options)
            : base(options) { }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>().OwnsOne(a => a.Cover);
            base.OnModelCreating(modelBuilder);
        }
    }
}
