using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Square.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_columns_topics_topic_id",
                schema: "domain",
                table: "columns");

            migrationBuilder.AddColumn<string>(
                name: "title",
                schema: "domain",
                table: "topics",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_topics__title",
                schema: "domain",
                table: "topics",
                column: "title",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_columns_topics__topic_id",
                schema: "domain",
                table: "columns",
                column: "topic_id",
                principalSchema: "domain",
                principalTable: "topics",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_columns_topics__topic_id",
                schema: "domain",
                table: "columns");

            migrationBuilder.DropIndex(
                name: "ix_topics__title",
                schema: "domain",
                table: "topics");

            migrationBuilder.DropColumn(
                name: "title",
                schema: "domain",
                table: "topics");

            migrationBuilder.AddForeignKey(
                name: "fk_columns_topics_topic_id",
                schema: "domain",
                table: "columns",
                column: "topic_id",
                principalSchema: "domain",
                principalTable: "topics",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
