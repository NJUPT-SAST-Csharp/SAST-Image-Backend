using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SastImg.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class Fix1 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(name: "ix_albums_category_id", table: "albums");

        migrationBuilder.CreateIndex(
            name: "ix_albums_category_id",
            table: "albums",
            column: "category_id"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(name: "ix_albums_category_id", table: "albums");

        migrationBuilder.CreateIndex(
            name: "ix_albums_category_id",
            table: "albums",
            column: "category_id",
            unique: true
        );
    }
}
