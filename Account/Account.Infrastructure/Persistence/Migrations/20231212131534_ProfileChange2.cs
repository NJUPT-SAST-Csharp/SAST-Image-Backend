using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Account.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProfileChange2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_profile_users_id",
                table: "profile");

            migrationBuilder.DropPrimaryKey(
                name: "pk_profile",
                table: "profile");

            migrationBuilder.RenameTable(
                name: "profile",
                newName: "profiles");

            migrationBuilder.AddPrimaryKey(
                name: "pk_profiles",
                table: "profiles",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_profiles_users_id",
                table: "profiles",
                column: "id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_profiles_users_id",
                table: "profiles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_profiles",
                table: "profiles");

            migrationBuilder.RenameTable(
                name: "profiles",
                newName: "profile");

            migrationBuilder.AddPrimaryKey(
                name: "pk_profile",
                table: "profile",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_profile_users_id",
                table: "profile",
                column: "id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
