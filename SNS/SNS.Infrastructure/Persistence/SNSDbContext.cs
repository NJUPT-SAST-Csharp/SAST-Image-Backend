using Microsoft.EntityFrameworkCore;
using SNS.Domain.AlbumEntity;
using SNS.Domain.ImageAggregate.ImageEntity;
using SNS.Domain.UserEntity;
using SNS.Infrastructure.EntityTypeConfigurations;

namespace SNS.Infrastructure.Persistence
{
    public sealed class SNSDbContext(DbContextOptions<SNSDbContext> options) : DbContext(options)
    {
        public DbSet<Album> Albums { get; init; }
        public DbSet<User> Users { get; init; }
        public DbSet<Image> Images { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AlbumEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ImageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
