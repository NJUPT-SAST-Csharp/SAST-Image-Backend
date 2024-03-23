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
            migrationBuilder.DropForeignKey(
                name: "fk_columns_topics_topic_id",
                table: "columns");

            migrationBuilder.RenameIndex(
                name: "ix_columns_topic_id",
                table: "columns",
                newName: "ix_columns__topic_id");

            migrationBuilder.AddForeignKey(
                name: "fk_columns_topics__topic_id",
                table: "columns",
                column: "topic_id",
                principalTable: "topics",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_columns_topics__topic_id",
                table: "columns");

            migrationBuilder.RenameIndex(
                name: "ix_columns__topic_id",
                table: "columns",
                newName: "ix_columns_topic_id");

            migrationBuilder.AddForeignKey(
                name: "fk_columns_topics_topic_id",
                table: "columns",
                column: "topic_id",
                principalTable: "topics",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
