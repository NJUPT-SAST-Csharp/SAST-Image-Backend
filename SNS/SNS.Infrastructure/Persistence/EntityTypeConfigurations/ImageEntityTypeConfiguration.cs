using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SNS.Domain.AlbumEntity;
using SNS.Domain.ImageAggregate.ImageEntity;
using SNS.Domain.UserEntity;

namespace SNS.Infrastructure.Persistence.EntityTypeConfigurations
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

            builder.OwnsMany<Like>(
                "_likes",
                like =>
                {
                    like.ToTable("likes");
                    like.HasKey(l => new { l.ImageId, l.UserId });

                    like.WithOwner().HasForeignKey(x => x.ImageId);

                    like.Property(l => l.ImageId)
                        .HasColumnName("image_id")
                        .HasConversion(x => x.Value, x => new(x));

                    like.Property(l => l.UserId)
                        .HasColumnName("liker_id")
                        .HasConversion(id => id.Value, id => new(id));

                    like.Property(l => l.LikeAt).HasColumnName("like_at");
                }
            );

            builder.OwnsMany<Favourite>(
                "_favourites",
                favourite =>
                {
                    favourite.ToTable("favourites");
                    favourite.HasKey(f => new { f.ImageId, f.UserId });

                    favourite.WithOwner().HasForeignKey(x => x.ImageId);

                    favourite
                        .Property(f => f.ImageId)
                        .HasColumnName("image_id")
                        .HasConversion(x => x.Value, x => new(x));

                    favourite
                        .Property(f => f.UserId)
                        .HasColumnName("favouriter_id")
                        .HasConversion(id => id.Value, id => new(id));

                    favourite.Property(f => f.FavouriteAt).HasColumnName("favourite_at");
                }
            );

            builder.OwnsMany<Comment>(
                "_comments",
                comment =>
                {
                    comment.ToTable("comments");
                    comment.HasKey(c => new { c.ImageId, c.CommenterId });

                    comment.WithOwner().HasForeignKey(x => x.ImageId);

                    comment
                        .Property(c => c.ImageId)
                        .HasColumnName("image_id")
                        .HasConversion(x => x.Value, x => new(x));

                    comment
                        .Property(c => c.CommenterId)
                        .HasColumnName("commenter_id")
                        .HasConversion(id => id.Value, id => new(id));

                    comment.Property(c => c.Content).HasColumnName("content");
                    comment.Property(c => c.CommentAt).HasColumnName("comment_at");
                }
            );
        }
    }
}
