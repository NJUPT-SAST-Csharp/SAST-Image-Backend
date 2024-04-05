using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Square.Infrastructure.Persistence.QueryMigrations
{
    /// <inheritdoc />
    public partial class QueryFix4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "category_id",
                schema: "query",
                table: "topics",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "query",
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
                name: "ix_topics_category_id",
                schema: "query",
                table: "topics",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_categories_name",
                schema: "query",
                table: "categories",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_topics_categories_category_id",
                schema: "query",
                table: "topics",
                column: "category_id",
                principalSchema: "query",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_topics_categories_category_id",
                schema: "query",
                table: "topics");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "query");

            migrationBuilder.DropIndex(
                name: "ix_topics_category_id",
                schema: "query",
                table: "topics");

            migrationBuilder.DropColumn(
                name: "category_id",
                schema: "query",
                table: "topics");
        }
    }
}
