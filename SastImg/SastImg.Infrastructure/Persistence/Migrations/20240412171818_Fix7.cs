using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SastImg.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class Fix7 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "bookmarks");

        migrationBuilder.DropTable(name: "likes");

        migrationBuilder.DropTable(name: "subscribers");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "bookmarks",
            columns: table => new
            {
                image = table.Column<long>(type: "bigint", nullable: false),
                user = table.Column<long>(type: "bigint", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_bookmarks", x => new { x.image, x.user });
                table.ForeignKey(
                    name: "fk_bookmarks_images_image",
                    column: x => x.image,
                    principalTable: "images",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "likes",
            columns: table => new
            {
                image = table.Column<long>(type: "bigint", nullable: false),
                user = table.Column<long>(type: "bigint", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_likes", x => new { x.image, x.user });
                table.ForeignKey(
                    name: "fk_likes_images_image",
                    column: x => x.image,
                    principalTable: "images",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "subscribers",
            columns: table => new
            {
                user = table.Column<long>(type: "bigint", nullable: false),
                album = table.Column<long>(type: "bigint", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_subscribers", x => new { x.user, x.album });
                table.ForeignKey(
                    name: "fk_subscribers_albums_album_id",
                    column: x => x.album,
                    principalTable: "albums",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateIndex(
            name: "ix_subscribers_album",
            table: "subscribers",
            column: "album"
        );
    }
}
