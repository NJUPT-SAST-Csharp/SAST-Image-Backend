using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Square.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_column_likes",
                table: "column_likes");

            migrationBuilder.AddPrimaryKey(
                name: "pk_column_likes",
                table: "column_likes",
                columns: new[] { "user_id", "column_id" });

            migrationBuilder.CreateTable(
                name: "topic_likes",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    topic_id = table.Column<long>(type: "bigint", nullable: false),
                    liked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topic_likes", x => new { x.user_id, x.topic_id });
                    table.ForeignKey(
                        name: "fk_topic_likes_topics_topic_id",
                        column: x => x.topic_id,
                        principalTable: "topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_column_likes_column_id",
                table: "column_likes",
                column: "column_id");

            migrationBuilder.CreateIndex(
                name: "ix_topic_likes_topic_id",
                table: "topic_likes",
                column: "topic_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "topic_likes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_column_likes",
                table: "column_likes");

            migrationBuilder.DropIndex(
                name: "ix_column_likes_column_id",
                table: "column_likes");

            migrationBuilder.AddPrimaryKey(
                name: "pk_column_likes",
                table: "column_likes",
                columns: new[] { "column_id", "user_id" });
        }
    }
}
