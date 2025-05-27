using Microsoft.EntityFrameworkCore;
using SNS.Domain.Bookmarks;
using SNS.Domain.Follows;
using SNS.Domain.Likes;
using SNS.Domain.Subscribes;

namespace SNS.Infrastructure.Persistence;

public sealed class SNSDbContext(DbContextOptions<SNSDbContext> options) : DbContext(options)
{
    internal DbSet<Like> Likes { get; init; }
    internal DbSet<Follow> Follows { get; init; }
    internal DbSet<Subscribe> Subscribes { get; init; }
    internal DbSet<Bookmark> Bookmarks { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Like>(builder =>
        {
            builder.HasKey(l => new { l.UserId, l.ImageId });
            builder
                .Property(l => l.UserId)
                .HasColumnName("user")
                .HasConversion(u => u.Value, u => new(u));
            builder
                .Property(l => l.ImageId)
                .HasColumnName("image")
                .HasConversion(i => i.Value, i => new(i));
        });

        modelBuilder.Entity<Follow>(builder =>
        {
            builder.HasKey(f => new { f.Follower, f.Following });
            builder
                .Property(f => f.Follower)
                .HasColumnName("follower")
                .HasConversion(u => u.Value, u => new(u));
            builder
                .Property(f => f.Following)
                .HasColumnName("following")
                .HasConversion(u => u.Value, u => new(u));
        });

        modelBuilder.Entity<Subscribe>(builder =>
        {
            builder.HasKey(s => new { s.UserId, s.AlbumId });
            builder
                .Property(s => s.UserId)
                .HasColumnName("user")
                .HasConversion(u => u.Value, u => new(u));
            builder
                .Property(s => s.AlbumId)
                .HasColumnName("album")
                .HasConversion(u => u.Value, u => new(u));
        });

        modelBuilder.Entity<Bookmark>(builder =>
        {
            builder.HasKey(b => new { b.UserId, b.ImageId });
            builder
                .Property(b => b.UserId)
                .HasColumnName("user")
                .HasConversion(u => u.Value, u => new(u));
            builder
                .Property(b => b.ImageId)
                .HasColumnName("image")
                .HasConversion(i => i.Value, i => new(i));
        });
    }
}
