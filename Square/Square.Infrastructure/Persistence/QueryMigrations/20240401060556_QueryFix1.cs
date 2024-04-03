using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Square.Infrastructure.Persistence.QueryMigrations
{
    /// <inheritdoc />
    public partial class QueryFix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "column_likes",
                schema: "query",
                columns: table => new
                {
                    column_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_column_likes", x => new { x.column_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_column_likes_columns_column_id",
                        column: x => x.column_id,
                        principalSchema: "query",
                        principalTable: "columns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "column_likes",
                schema: "query");
        }
    }
}
