using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SNS.Domain.AlbumEntity;
using SNS.Domain.ImageAggregate.CommentEntity;
using SNS.Domain.ImageAggregate.ImageEntity;
using SNS.Domain.UserEntity;

namespace SNS.Infrastructure.EntityTypeConfigurations
{
    internal sealed class ImageEntityTypeConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("images");
            builder.HasKey(x => x.Id);
            builder.Ignore(x => x.DomainEvents);

            builder
                .Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(x => x.Value, x => new ImageId(x));
            builder
                .Property<UserId>("_authorId")
                .HasColumnName("author_id")
                .HasConversion(x => x.Value, x => new UserId(x));
            builder
                .Property<AlbumId>("_albumId")
                .HasColumnName("album_id")
                .HasConversion(x => x.Value, x => new AlbumId(x));

            builder
                .HasMany<User>()
                .WithMany()
                .UsingEntity<Like>(
                    left => left.HasOne<User>().WithMany().HasForeignKey(x => x.UserId),
                    right =>
                        right.HasOne<Image>().WithMany("_likedBy").HasForeignKey(x => x.ImageId),
                    like =>
                    {
                        like.Property(x => x.UserId)
                            .HasColumnName("liker_id")
                            .HasConversion(x => x.Value, x => new UserId(x));
                        like.Property(x => x.ImageId)
                            .HasColumnName("image_id")
                            .HasConversion(id => id.Value, id => new ImageId(id));
                    }
                );
            builder
                .HasMany<User>()
                .WithMany()
                .UsingEntity<Favourite>(
                    left => left.HasOne<User>().WithMany().HasForeignKey(x => x.UserId),
                    right =>
                        right
                            .HasOne<Image>()
                            .WithMany("_favouritedBy")
                            .HasForeignKey(x => x.ImageId),
                    fav =>
                    {
                        fav.Property(x => x.UserId)
                            .HasColumnName("favouriter_id")
                            .HasConversion(x => x.Value, x => new UserId(x));
                        fav.Property(x => x.ImageId)
                            .HasColumnName("image_id")
                            .HasConversion(x => x.Value, x => new ImageId(x));
                    }
                );

            builder.HasOne<User>().WithMany().HasForeignKey("_authorId");
            builder.HasOne<Album>().WithMany().HasForeignKey("_albumId");

            builder.OwnsMany<Comment>(
                "_comments",
                comment =>
                {
                    comment.ToTable("comments");
                    comment.HasKey(c => c.Id);

                    comment.Ignore(c => c.DomainEvents);
                    comment.WithOwner().HasForeignKey("image_id");

                    comment
                        .Property(c => c.Id)
                        .HasColumnName("id")
                        .HasConversion(x => x.Value, x => new(x));
                    comment
                        .Property<UserId>("_commenter")
                        .HasColumnName("commenter")
                        .HasConversion(id => id.Value, id => new(id));

                    comment.Property<string>("content").HasColumnName("content");
                    comment.Property<DateTime>("_commentAt").HasColumnName("comment_at");

                    comment.HasOne<User>().WithMany().HasForeignKey("_authorId");
                }
            );
        }
    }
}
