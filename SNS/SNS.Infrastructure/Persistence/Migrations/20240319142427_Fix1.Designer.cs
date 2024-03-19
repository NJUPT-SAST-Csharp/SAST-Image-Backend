﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SNS.Infrastructure.Persistence;

#nullable disable

namespace SNS.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(SNSDbContext))]
    [Migration("20240319142427_Fix1")]
    partial class Fix1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SNS.Domain.AlbumEntity.Album", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<long>("_authorId")
                        .HasColumnType("bigint")
                        .HasColumnName("author_id");

                    b.HasKey("Id")
                        .HasName("pk_albums");

                    b.ToTable("albums", (string)null);
                });

            modelBuilder.Entity("SNS.Domain.ImageAggregate.ImageEntity.Image", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<long>("_albumId")
                        .HasColumnType("bigint")
                        .HasColumnName("album_id");

                    b.Property<long>("_authorId")
                        .HasColumnType("bigint")
                        .HasColumnName("author_id");

                    b.HasKey("Id")
                        .HasName("pk_images");

                    b.HasIndex("_albumId")
                        .HasDatabaseName("ix_images__album_id");

                    b.ToTable("images", (string)null);
                });

            modelBuilder.Entity("SNS.Domain.UserEntity.User", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.Property<long>("follower")
                        .HasColumnType("bigint")
                        .HasColumnName("follower");

                    b.Property<long>("following")
                        .HasColumnType("bigint")
                        .HasColumnName("following");

                    b.HasKey("follower", "following")
                        .HasName("pk_followers");

                    b.HasIndex("following")
                        .HasDatabaseName("ix_followers_following");

                    b.ToTable("followers", (string)null);
                });

            modelBuilder.Entity("SNS.Domain.AlbumEntity.Album", b =>
                {
                    b.OwnsMany("SNS.Domain.AlbumEntity.Subscriber", "_subscribers", b1 =>
                        {
                            b1.Property<long>("AlbumId")
                                .HasColumnType("bigint")
                                .HasColumnName("album_id");

                            b1.Property<long>("SubscriberId")
                                .HasColumnType("bigint")
                                .HasColumnName("subscriber_id");

                            b1.Property<DateTime>("SubscribeAt")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("subscribe_at");

                            b1.HasKey("AlbumId", "SubscriberId")
                                .HasName("pk_subscribers");

                            b1.ToTable("subscribers", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("AlbumId")
                                .HasConstraintName("fk_subscribers_albums_album_id");
                        });

                    b.Navigation("_subscribers");
                });

            modelBuilder.Entity("SNS.Domain.ImageAggregate.ImageEntity.Image", b =>
                {
                    b.HasOne("SNS.Domain.AlbumEntity.Album", null)
                        .WithMany()
                        .HasForeignKey("_albumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_images_albums__album_id");

                    b.OwnsMany("SNS.Domain.ImageAggregate.ImageEntity.Comment", "_comments", b1 =>
                        {
                            b1.Property<long>("ImageId")
                                .HasColumnType("bigint")
                                .HasColumnName("image_id");

                            b1.Property<long>("CommenterId")
                                .HasColumnType("bigint")
                                .HasColumnName("commenter_id");

                            b1.Property<DateTime>("CommentAt")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("comment_at");

                            b1.Property<string>("Content")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("content");

                            b1.HasKey("ImageId", "CommenterId")
                                .HasName("pk_comments");

                            b1.ToTable("comments", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ImageId")
                                .HasConstraintName("fk_comments_images_image_id");
                        });

                    b.OwnsMany("SNS.Domain.ImageAggregate.ImageEntity.Favourite", "_favourites", b1 =>
                        {
                            b1.Property<long>("ImageId")
                                .HasColumnType("bigint")
                                .HasColumnName("image_id");

                            b1.Property<long>("UserId")
                                .HasColumnType("bigint")
                                .HasColumnName("favouriter_id");

                            b1.Property<DateTime>("FavouriteAt")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("favourite_at");

                            b1.HasKey("ImageId", "UserId")
                                .HasName("pk_favourites");

                            b1.ToTable("favourites", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ImageId")
                                .HasConstraintName("fk_favourites_images_image_id");
                        });

                    b.OwnsMany("SNS.Domain.ImageAggregate.ImageEntity.Like", "_likes", b1 =>
                        {
                            b1.Property<long>("ImageId")
                                .HasColumnType("bigint")
                                .HasColumnName("image_id");

                            b1.Property<long>("UserId")
                                .HasColumnType("bigint")
                                .HasColumnName("liker_id");

                            b1.Property<DateTime>("LikeAt")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("like_at");

                            b1.HasKey("ImageId", "UserId")
                                .HasName("pk_likes");

                            b1.ToTable("likes", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ImageId")
                                .HasConstraintName("fk_likes_images_image_id");
                        });

                    b.Navigation("_comments");

                    b.Navigation("_favourites");

                    b.Navigation("_likes");
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.HasOne("SNS.Domain.UserEntity.User", null)
                        .WithMany()
                        .HasForeignKey("follower")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_followers_users_follower");

                    b.HasOne("SNS.Domain.UserEntity.User", null)
                        .WithMany()
                        .HasForeignKey("following")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_followers_users_following");
                });
#pragma warning restore 612, 618
        }
    }
}
