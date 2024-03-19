using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SNS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class REFACTOR2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_albums_users_author_id",
                table: "albums");

            migrationBuilder.DropForeignKey(
                name: "fk_comments_users__author_id",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "fk_favourites_users_favouriter_id",
                table: "favourites");

            migrationBuilder.DropForeignKey(
                name: "fk_followers_users_follower",
                table: "followers");

            migrationBuilder.DropForeignKey(
                name: "fk_followers_users_following",
                table: "followers");

            migrationBuilder.DropForeignKey(
                name: "fk_images_users_author_id",
                table: "images");

            migrationBuilder.DropForeignKey(
                name: "fk_likes_users_liker_id",
                table: "likes");

            migrationBuilder.DropForeignKey(
                name: "fk_subscribers_users_subscriber_id",
                table: "subscribers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_subscribers",
                table: "subscribers");

            migrationBuilder.DropIndex(
                name: "ix_subscribers_album_id",
                table: "subscribers");

            migrationBuilder.DropIndex(
                name: "ix_likes_user_id",
                table: "likes");

            migrationBuilder.DropIndex(
                name: "ix_images_author_id",
                table: "images");

            migrationBuilder.DropPrimaryKey(
                name: "pk_followers",
                table: "followers");

            migrationBuilder.DropIndex(
                name: "ix_favourites_user_id",
                table: "favourites");

            migrationBuilder.DropPrimaryKey(
                name: "pk_comments",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "ix_comments__author_id",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "ix_comments_image_id",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "ix_albums_author_id",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "id",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "_author_id",
                table: "comments");

            migrationBuilder.RenameColumn(
                name: "following",
                table: "followers",
                newName: "_following_id");

            migrationBuilder.RenameColumn(
                name: "follower",
                table: "followers",
                newName: "following_id");

            migrationBuilder.RenameIndex(
                name: "ix_followers_following",
                table: "followers",
                newName: "ix_followers__following_id");

            migrationBuilder.RenameColumn(
                name: "commenter",
                table: "comments",
                newName: "commenter_id");

            migrationBuilder.AddColumn<DateTime>(
                name: "subscribe_at",
                table: "subscribers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "like_at",
                table: "likes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "follower_id",
                table: "followers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "favourite_at",
                table: "favourites",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "comments",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_subscribers",
                table: "subscribers",
                columns: new[] { "album_id", "subscriber_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_followers",
                table: "followers",
                columns: new[] { "follower_id", "following_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_comments",
                table: "comments",
                columns: new[] { "image_id", "commenter_id" });

            migrationBuilder.CreateIndex(
                name: "ix_followers_following_id",
                table: "followers",
                column: "following_id");

            migrationBuilder.AddForeignKey(
                name: "fk_followers_users__following_id",
                table: "followers",
                column: "_following_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_followers_users_follower_id",
                table: "followers",
                column: "follower_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_followers_users_following_id",
                table: "followers",
                column: "following_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_followers_users__following_id",
                table: "followers");

            migrationBuilder.DropForeignKey(
                name: "fk_followers_users_follower_id",
                table: "followers");

            migrationBuilder.DropForeignKey(
                name: "fk_followers_users_following_id",
                table: "followers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_subscribers",
                table: "subscribers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_followers",
                table: "followers");

            migrationBuilder.DropIndex(
                name: "ix_followers_following_id",
                table: "followers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_comments",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "subscribe_at",
                table: "subscribers");

            migrationBuilder.DropColumn(
                name: "like_at",
                table: "likes");

            migrationBuilder.DropColumn(
                name: "follower_id",
                table: "followers");

            migrationBuilder.DropColumn(
                name: "favourite_at",
                table: "favourites");

            migrationBuilder.RenameColumn(
                name: "_following_id",
                table: "followers",
                newName: "following");

            migrationBuilder.RenameColumn(
                name: "following_id",
                table: "followers",
                newName: "follower");

            migrationBuilder.RenameIndex(
                name: "ix_followers__following_id",
                table: "followers",
                newName: "ix_followers_following");

            migrationBuilder.RenameColumn(
                name: "commenter_id",
                table: "comments",
                newName: "commenter");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "comments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<long>(
                name: "id",
                table: "comments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "_author_id",
                table: "comments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_subscribers",
                table: "subscribers",
                column: "subscriber_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_followers",
                table: "followers",
                columns: new[] { "follower", "following" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_comments",
                table: "comments",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_subscribers_album_id",
                table: "subscribers",
                column: "album_id");

            migrationBuilder.CreateIndex(
                name: "ix_likes_user_id",
                table: "likes",
                column: "liker_id");

            migrationBuilder.CreateIndex(
                name: "ix_images_author_id",
                table: "images",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_favourites_user_id",
                table: "favourites",
                column: "favouriter_id");

            migrationBuilder.CreateIndex(
                name: "ix_comments__author_id",
                table: "comments",
                column: "_author_id");

            migrationBuilder.CreateIndex(
                name: "ix_comments_image_id",
                table: "comments",
                column: "image_id");

            migrationBuilder.CreateIndex(
                name: "ix_albums_author_id",
                table: "albums",
                column: "author_id");

            migrationBuilder.AddForeignKey(
                name: "fk_albums_users_author_id",
                table: "albums",
                column: "author_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_comments_users__author_id",
                table: "comments",
                column: "_author_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_favourites_users_favouriter_id",
                table: "favourites",
                column: "favouriter_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_followers_users_follower",
                table: "followers",
                column: "follower",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_followers_users_following",
                table: "followers",
                column: "following",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_images_users_author_id",
                table: "images",
                column: "author_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_likes_users_liker_id",
                table: "likes",
                column: "liker_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_subscribers_users_subscriber_id",
                table: "subscribers",
                column: "subscriber_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
