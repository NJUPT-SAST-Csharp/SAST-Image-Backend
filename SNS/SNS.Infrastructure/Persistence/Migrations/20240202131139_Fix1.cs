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
                name: "fk_user_user_users_follower",
                table: "user_user");

            migrationBuilder.DropForeignKey(
                name: "fk_user_user_users_following",
                table: "user_user");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_user",
                table: "user_user");

            migrationBuilder.RenameTable(
                name: "user_user",
                newName: "followers");

            migrationBuilder.RenameIndex(
                name: "ix_user_user_following",
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

            migrationBuilder.RenameTable(
                name: "followers",
                newName: "user_user");

            migrationBuilder.RenameIndex(
                name: "ix_followers_following",
                table: "user_user",
                newName: "ix_user_user_following");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_user",
                table: "user_user",
                columns: new[] { "follower", "following" });

            migrationBuilder.AddForeignKey(
                name: "fk_user_user_users_follower",
                table: "user_user",
                column: "follower",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_user_users_following",
                table: "user_user",
                column: "following",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
