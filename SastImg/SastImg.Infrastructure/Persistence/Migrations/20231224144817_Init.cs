using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SastImg.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "categories",
            columns: table => new
            {
                id = table
                    .Column<long>(type: "bigint", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                    ),
                description = table.Column<string>(type: "text", nullable: false),
                name = table.Column<string>(type: "text", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_categories", x => x.id);
            }
        );

        migrationBuilder.CreateTable(
            name: "tags",
            columns: table => new
            {
                id = table
                    .Column<long>(type: "bigint", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                    ),
                name = table.Column<string>(type: "text", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_tags", x => x.id);
            }
        );

        migrationBuilder.CreateTable(
            name: "albums",
            columns: table => new
            {
                id = table
                    .Column<long>(type: "bigint", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                    ),
                accessibility = table.Column<int>(type: "integer", nullable: false),
                author_id = table.Column<long>(type: "bigint", nullable: false),
                category_id = table.Column<long>(type: "bigint", nullable: false),
                collaborators = table.Column<List<long>>(
                    type: "bigint[]",
                    unicode: false,
                    maxLength: 512,
                    nullable: false
                ),
                created_at = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: false
                ),
                description = table.Column<string>(type: "text", nullable: false),
                is_removed = table.Column<bool>(type: "boolean", nullable: false),
                title = table.Column<string>(type: "text", nullable: false),
                updated_at = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: false
                ),
                cover_url = table.Column<string>(type: "text", nullable: true),
                cover_is_latest_image = table.Column<bool>(type: "boolean", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_albums", x => x.id);
                table.ForeignKey(
                    name: "fk_albums_categories_category_id",
                    column: x => x.category_id,
                    principalTable: "categories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "images",
            columns: table => new
            {
                id = table
                    .Column<long>(type: "bigint", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                    ),
                album_id = table.Column<long>(type: "bigint", nullable: false),
                description = table.Column<string>(type: "text", nullable: false),
                is_nsfw = table.Column<bool>(type: "boolean", nullable: false),
                is_removed = table.Column<bool>(type: "boolean", nullable: false),
                tags = table.Column<List<long>>(
                    type: "bigint[]",
                    unicode: false,
                    maxLength: 512,
                    nullable: false
                ),
                title = table.Column<string>(type: "text", nullable: false),
                uploaded_at = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: false
                ),
                url = table.Column<string>(type: "text", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_images", x => new { x.album_id, x.id });
                table.ForeignKey(
                    name: "fk_images_albums_album_id",
                    column: x => x.album_id,
                    principalTable: "albums",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateIndex(
            name: "ix_albums_category_id",
            table: "albums",
            column: "category_id",
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "ix_categories__name",
            table: "categories",
            column: "name",
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "ix_tags__name",
            table: "tags",
            column: "name",
            unique: true
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "images");

        migrationBuilder.DropTable(name: "tags");

        migrationBuilder.DropTable(name: "albums");

        migrationBuilder.DropTable(name: "categories");
    }
}
