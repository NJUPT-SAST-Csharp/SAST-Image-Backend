using Microsoft.EntityFrameworkCore;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.CategoryEntity;
using SastImg.Domain.TagEntity;
using SastImg.Infrastructure.Domain.AlbumEntity;
using SastImg.Infrastructure.Persistence.EntityTypeConfigurations;

namespace SastImg.Infrastructure.Persistence
{
    public sealed class SastImgDbContext(DbContextOptions<SastImgDbContext> options)
        : DbContext(options)
    {
        public DbSet<Album> Albums { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AlbumEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TagEntityTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
