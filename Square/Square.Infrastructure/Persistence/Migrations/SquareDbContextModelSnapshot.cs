﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Square.Infrastructure.Persistence;

#nullable disable

namespace Square.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(SquareDbContext))]
    partial class SquareDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Square.Domain.TopicAggregate.TopicEntity.Topic", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<long>("_authorId")
                        .HasColumnType("bigint")
                        .HasColumnName("author_id");

                    b.Property<string>("_description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<DateTime>("_publishedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("published_at");

                    b.Property<string>("_title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<DateTime>("_updatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_topics");

                    b.ToTable("topics", (string)null);
                });

            modelBuilder.Entity("Square.Domain.TopicAggregate.TopicEntity.Topic", b =>
                {
                    b.OwnsMany("Square.Domain.TopicAggregate.ColumnEntity.Column", "_columns", b1 =>
                        {
                            b1.Property<long>("Id")
                                .HasColumnType("bigint")
                                .HasColumnName("column_id");

                            b1.Property<long>("TopicId")
                                .HasColumnType("bigint")
                                .HasColumnName("topic_id");

                            b1.Property<long>("_authorId")
                                .HasColumnType("bigint")
                                .HasColumnName("author_id");

                            b1.Property<string>("_text")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("text");

                            b1.Property<DateTime>("_uploadedAt")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("uploaded_at");

                            b1.HasKey("Id")
                                .HasName("pk_columns");

                            b1.HasIndex("TopicId")
                                .HasDatabaseName("ix_columns_topic_id");

                            b1.ToTable("columns", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("TopicId")
                                .HasConstraintName("fk_columns_topics_topic_id");

                            b1.OwnsMany("Square.Domain.TopicAggregate.ColumnEntity.TopicImage", "_images", b2 =>
                                {
                                    b2.Property<long>("Id")
                                        .HasColumnType("bigint")
                                        .HasColumnName("id");

                                    b2.Property<long>("ColumnId")
                                        .HasColumnType("bigint")
                                        .HasColumnName("column_id");

                                    b2.Property<string>("Url")
                                        .IsRequired()
                                        .HasColumnType("text")
                                        .HasColumnName("image_url");

                                    b2.HasKey("Id")
                                        .HasName("pk_column_images");

                                    b2.HasIndex("ColumnId")
                                        .HasDatabaseName("ix_column_images_column_id");

                                    b2.ToTable("column_images", (string)null);

                                    b2.WithOwner()
                                        .HasForeignKey("ColumnId")
                                        .HasConstraintName("fk_column_images_columns_column_id");
                                });

                            b1.OwnsMany("Square.Domain.TopicAggregate.Like", "_likes", b2 =>
                                {
                                    b2.Property<long>("UserId")
                                        .HasColumnType("bigint")
                                        .HasColumnName("user_id");

                                    b2.Property<long>("column_id")
                                        .HasColumnType("bigint")
                                        .HasColumnName("column_id");

                                    b2.Property<DateTime>("LikedAt")
                                        .HasColumnType("timestamp with time zone")
                                        .HasColumnName("liked_at");

                                    b2.HasKey("UserId", "column_id")
                                        .HasName("pk_column_likes");

                                    b2.HasIndex("column_id")
                                        .HasDatabaseName("ix_column_likes_column_id");

                                    b2.ToTable("column_likes", (string)null);

                                    b2.WithOwner()
                                        .HasForeignKey("column_id")
                                        .HasConstraintName("fk_column_likes_columns_column_id");
                                });

                            b1.Navigation("_images");

                            b1.Navigation("_likes");
                        });

                    b.OwnsMany("Square.Domain.TopicAggregate.TopicEntity.Subscribe", "_subscribers", b1 =>
                        {
                            b1.Property<long>("UserId")
                                .HasColumnType("bigint")
                                .HasColumnName("user_id");

                            b1.Property<long>("TopicId")
                                .HasColumnType("bigint")
                                .HasColumnName("topic_id");

                            b1.Property<DateTime>("SubscribedAt")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("subscribed_at");

                            b1.HasKey("UserId", "TopicId")
                                .HasName("pk_subscribers");

                            b1.HasIndex("TopicId")
                                .HasDatabaseName("ix_subscribers_topic_id");

                            b1.ToTable("subscribers", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("TopicId")
                                .HasConstraintName("fk_subscribers_topics_topic_id");
                        });

                    b.OwnsMany("Square.Domain.TopicAggregate.Like", "_likes", b1 =>
                        {
                            b1.Property<long>("UserId")
                                .HasColumnType("bigint")
                                .HasColumnName("user_id");

                            b1.Property<long>("topic_id")
                                .HasColumnType("bigint")
                                .HasColumnName("topic_id");

                            b1.Property<DateTime>("LikedAt")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("liked_at");

                            b1.HasKey("UserId", "topic_id");

                            b1.HasIndex("topic_id")
                                .HasDatabaseName("ix_topic_likes_topic_id");

                            b1.ToTable("topic_likes", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("topic_id")
                                .HasConstraintName("fk_topic_likes_topics_topic_id");
                        });

                    b.Navigation("_columns");

                    b.Navigation("_likes");

                    b.Navigation("_subscribers");
                });
#pragma warning restore 612, 618
        }
    }
}
