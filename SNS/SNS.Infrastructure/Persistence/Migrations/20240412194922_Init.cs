using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SNS.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "bookmarks",
            columns: table => new
            {
                user = table.Column<long>(type: "bigint", nullable: false),
                image = table.Column<long>(type: "bigint", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_bookmarks", x => new { x.user, x.image });
            }
        );

        migrationBuilder.CreateTable(
            name: "follows",
            columns: table => new
            {
                follower = table.Column<long>(type: "bigint", nullable: false),
                following = table.Column<long>(type: "bigint", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_follows", x => new { x.follower, x.following });
            }
        );

        migrationBuilder.CreateTable(
            name: "likes",
            columns: table => new
            {
                user = table.Column<long>(type: "bigint", nullable: false),
                image = table.Column<long>(type: "bigint", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_likes", x => new { x.user, x.image });
            }
        );

        migrationBuilder.CreateTable(
            name: "subscribes",
            columns: table => new
            {
                user = table.Column<long>(type: "bigint", nullable: false),
                album = table.Column<long>(type: "bigint", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_subscribes", x => new { x.user, x.album });
            }
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "bookmarks");

        migrationBuilder.DropTable(name: "follows");

        migrationBuilder.DropTable(name: "likes");

        migrationBuilder.DropTable(name: "subscribes");
    }
}
