using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SastImgAPI.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "tag_id",
                table: "images",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "header",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "cover_id",
                table: "albums",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tags", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_images_tag_id",
                table: "images",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "ix_albums_cover_id",
                table: "albums",
                column: "cover_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tags_name",
                table: "tags",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_albums_images_cover_id",
                table: "albums",
                column: "cover_id",
                principalTable: "images",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_images_tags_tag_id",
                table: "images",
                column: "tag_id",
                principalTable: "tags",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_albums_images_cover_id",
                table: "albums");

            migrationBuilder.DropForeignKey(
                name: "fk_images_tags_tag_id",
                table: "images");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropIndex(
                name: "ix_images_tag_id",
                table: "images");

            migrationBuilder.DropIndex(
                name: "ix_albums_cover_id",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "tag_id",
                table: "images");

            migrationBuilder.DropColumn(
                name: "header",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "cover_id",
                table: "albums");
        }
    }
}
