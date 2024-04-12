using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SastImg.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fix6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "thumbnail_url",
                table: "images");

            migrationBuilder.AlterColumn<string>(
                name: "url",
                table: "images",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "thumbnail",
                table: "images",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "bookmarks",
                columns: table => new
                {
                    user = table.Column<long>(type: "bigint", nullable: false),
                    image = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bookmarks", x => new { x.image, x.user });
                    table.ForeignKey(
                        name: "fk_bookmarks_images_image",
                        column: x => x.image,
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "likes",
                columns: table => new
                {
                    user = table.Column<long>(type: "bigint", nullable: false),
                    image = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_likes", x => new { x.image, x.user });
                    table.ForeignKey(
                        name: "fk_likes_images_image",
                        column: x => x.image,
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subscribers",
                columns: table => new
                {
                    user = table.Column<long>(type: "bigint", nullable: false),
                    album = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscribers", x => new { x.user, x.album });
                    table.ForeignKey(
                        name: "fk_subscribers_albums_album_id",
                        column: x => x.album,
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_subscribers_album",
                table: "subscribers",
                column: "album");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookmarks");

            migrationBuilder.DropTable(
                name: "likes");

            migrationBuilder.DropTable(
                name: "subscribers");

            migrationBuilder.DropColumn(
                name: "thumbnail",
                table: "images");

            migrationBuilder.AlterColumn<string>(
                name: "url",
                table: "images",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "thumbnail_url",
                table: "images",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
