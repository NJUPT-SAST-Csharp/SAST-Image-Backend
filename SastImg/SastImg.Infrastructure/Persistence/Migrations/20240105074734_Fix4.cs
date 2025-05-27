using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SastImg.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class Fix4 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "likes", table: "images");

        migrationBuilder.DropColumn(name: "views", table: "images");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "likes",
            table: "images",
            type: "integer",
            nullable: false,
            defaultValue: 0
        );

        migrationBuilder.AddColumn<int>(
            name: "views",
            table: "images",
            type: "integer",
            nullable: false,
            defaultValue: 0
        );
    }
}
