using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SastImg.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fix10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cover_url",
                table: "albums");

            migrationBuilder.AddColumn<long>(
                name: "cover_id",
                table: "albums",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cover_id",
                table: "albums");

            migrationBuilder.AddColumn<string>(
                name: "cover_url",
                table: "albums",
                type: "text",
                nullable: true);
        }
    }
}
