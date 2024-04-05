using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Square.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fix3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "_category_id",
                schema: "domain",
                table: "topics",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "domain",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_topics__category_id",
                schema: "domain",
                table: "topics",
                column: "_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_categories_name",
                schema: "domain",
                table: "categories",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_topics_categories__category_id",
                schema: "domain",
                table: "topics",
                column: "_category_id",
                principalSchema: "domain",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_topics_categories__category_id",
                schema: "domain",
                table: "topics");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "domain");

            migrationBuilder.DropIndex(
                name: "ix_topics__category_id",
                schema: "domain",
                table: "topics");

            migrationBuilder.DropColumn(
                name: "_category_id",
                schema: "domain",
                table: "topics");
        }
    }
}
