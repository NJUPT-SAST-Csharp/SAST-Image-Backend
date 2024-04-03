using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Square.Infrastructure.Persistence.QueryMigrations
{
    /// <inheritdoc />
    public partial class QueryFix3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_column_images_columns_id",
                schema: "query",
                table: "column_images");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "query",
                table: "column_images",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<long>(
                name: "column_id",
                schema: "query",
                table: "column_images",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "ix_column_images_column_id",
                schema: "query",
                table: "column_images",
                column: "column_id");

            migrationBuilder.AddForeignKey(
                name: "fk_column_images_columns_column_id",
                schema: "query",
                table: "column_images",
                column: "column_id",
                principalSchema: "query",
                principalTable: "columns",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_column_images_columns_column_id",
                schema: "query",
                table: "column_images");

            migrationBuilder.DropIndex(
                name: "ix_column_images_column_id",
                schema: "query",
                table: "column_images");

            migrationBuilder.DropColumn(
                name: "column_id",
                schema: "query",
                table: "column_images");

            migrationBuilder.AlterColumn<long>(
                name: "id",
                schema: "query",
                table: "column_images",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "fk_column_images_columns_id",
                schema: "query",
                table: "column_images",
                column: "id",
                principalSchema: "query",
                principalTable: "columns",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
