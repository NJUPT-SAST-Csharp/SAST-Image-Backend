using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SastImgAPI.Migrations
{
    /// <inheritdoc />
    public partial class _9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_albums_images_cover_id",
                table: "albums");

            migrationBuilder.DropIndex(
                name: "ix_albums_cover_id",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "cover_id",
                table: "albums");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "albums",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "cover",
                table: "albums",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cover",
                table: "albums");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "albums",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "cover_id",
                table: "albums",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_albums_cover_id",
                table: "albums",
                column: "cover_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_albums_images_cover_id",
                table: "albums",
                column: "cover_id",
                principalTable: "images",
                principalColumn: "id");
        }
    }
}
