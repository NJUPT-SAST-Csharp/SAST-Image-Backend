using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SNS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "pk_followers",
                table: "followers");

            migrationBuilder.DropIndex(
                name: "ix_followers_following_id",
                table: "followers");

            migrationBuilder.DropColumn(
                name: "follower_id",
                table: "followers");

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

            migrationBuilder.AddPrimaryKey(
                name: "pk_followers",
                table: "followers",
                columns: new[] { "follower", "following" });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_followers_users_follower",
                table: "followers");

            migrationBuilder.DropForeignKey(
                name: "fk_followers_users_following",
                table: "followers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_followers",
                table: "followers");

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

            migrationBuilder.AddColumn<long>(
                name: "follower_id",
                table: "followers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "pk_followers",
                table: "followers",
                columns: new[] { "follower_id", "following_id" });

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
    }
}
