using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SNS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    biography = table.Column<string>(type: "text", nullable: false),
                    nickname = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "albums",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    author_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_albums", x => x.id);
                    table.ForeignKey(
                        name: "fk_albums_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "followers",
                columns: table => new
                {
                    follower = table.Column<long>(type: "bigint", nullable: false),
                    following = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_followers", x => new { x.follower, x.following });
                    table.ForeignKey(
                        name: "fk_followers_users_follower",
                        column: x => x.follower,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_followers_users_following",
                        column: x => x.following,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    album_id = table.Column<long>(type: "bigint", nullable: false),
                    author_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_images_albums__album_id",
                        column: x => x.album_id,
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_images_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subscriber",
                columns: table => new
                {
                    subscriber_id = table.Column<long>(type: "bigint", nullable: false),
                    album_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscriber", x => x.subscriber_id);
                    table.ForeignKey(
                        name: "fk_subscriber_albums_album_id",
                        column: x => x.album_id,
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_subscriber_users_subscriber_id",
                        column: x => x.subscriber_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    _author_id = table.Column<long>(type: "bigint", nullable: true),
                    comment_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    commenter = table.Column<long>(type: "bigint", nullable: false),
                    content = table.Column<string>(type: "text", nullable: true),
                    image_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comments", x => x.id);
                    table.ForeignKey(
                        name: "fk_comments_images_image_id",
                        column: x => x.image_id,
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_comments_users__author_id",
                        column: x => x._author_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "favourites",
                columns: table => new
                {
                    image_id = table.Column<long>(type: "bigint", nullable: false),
                    favouriter_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_favourites", x => new { x.image_id, x.favouriter_id });
                    table.ForeignKey(
                        name: "fk_favourites_images_image_id",
                        column: x => x.image_id,
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_favourites_users_favouriter_id",
                        column: x => x.favouriter_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "likes",
                columns: table => new
                {
                    image_id = table.Column<long>(type: "bigint", nullable: false),
                    liker_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_likes", x => new { x.image_id, x.liker_id });
                    table.ForeignKey(
                        name: "fk_likes_images_image_id",
                        column: x => x.image_id,
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_likes_users_liker_id",
                        column: x => x.liker_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_albums_author_id",
                table: "albums",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_comments__author_id",
                table: "comments",
                column: "_author_id");

            migrationBuilder.CreateIndex(
                name: "ix_comments_image_id",
                table: "comments",
                column: "image_id");

            migrationBuilder.CreateIndex(
                name: "ix_favourites_user_id",
                table: "favourites",
                column: "favouriter_id");

            migrationBuilder.CreateIndex(
                name: "ix_followers_following",
                table: "followers",
                column: "following");

            migrationBuilder.CreateIndex(
                name: "ix_images__album_id",
                table: "images",
                column: "album_id");

            migrationBuilder.CreateIndex(
                name: "ix_images_author_id",
                table: "images",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_likes_user_id",
                table: "likes",
                column: "liker_id");

            migrationBuilder.CreateIndex(
                name: "ix_subscriber_album_id",
                table: "subscriber",
                column: "album_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "favourites");

            migrationBuilder.DropTable(
                name: "followers");

            migrationBuilder.DropTable(
                name: "likes");

            migrationBuilder.DropTable(
                name: "subscriber");

            migrationBuilder.DropTable(
                name: "images");

            migrationBuilder.DropTable(
                name: "albums");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
