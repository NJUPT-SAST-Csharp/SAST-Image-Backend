using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SNS.Domain.AlbumEntity;
using SNS.Domain.ImageAggregate.ImageEntity;
using SNS.Domain.UserEntity;

namespace SNS.Infrastructure.Persistence.EntityTypeConfigurations
{
    internal class AlbumEntityTypeConfiguration : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable("albums");

            builder.HasKey(a => a.Id);
            builder.Ignore(a => a.DomainEvents);

            builder
                .Property(a => a.Id)
                .HasColumnName("id")
                .HasConversion(x => x.Value, x => new AlbumId(x));
            builder
                .Property<UserId>("_authorId")
                .HasColumnName("author_id")
                .HasConversion(x => x.Value, x => new UserId(x));

            builder.OwnsMany<Subscribe>(
                "_subscribers",
                subscriber =>
                {
                    subscriber.ToTable("subscribers");
                    subscriber.HasKey(s => new { s.AlbumId, s.SubscriberId });

                    subscriber.WithOwner().HasForeignKey(s => s.AlbumId);

                    subscriber
                        .Property(s => s.AlbumId)
                        .HasColumnName("album_id")
                        .HasConversion(x => x.Value, x => new AlbumId(x));

                    subscriber
                        .Property(s => s.SubscriberId)
                        .HasColumnName("subscriber_id")
                        .HasConversion(x => x.Value, x => new UserId(x));

                    subscriber.Property(s => s.SubscribeAt).HasColumnName("subscribe_at");
                }
            );

            builder.HasMany<Image>().WithOne().HasForeignKey("_albumId");
        }
    }
}
