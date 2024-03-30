using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Square.Infrastructure.Persistence.QueryMigrations
{
    /// <inheritdoc />
    public partial class QueryInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "query");

            migrationBuilder.CreateTable(
                name: "topics",
                schema: "query",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    author_id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_topics", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "columns",
                schema: "query",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    author_id = table.Column<long>(type: "bigint", nullable: false),
                    topic_id = table.Column<long>(type: "bigint", nullable: false),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_columns", x => x.id);
                    table.ForeignKey(
                        name: "fk_columns_topics_topic_id",
                        column: x => x.topic_id,
                        principalSchema: "query",
                        principalTable: "topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "topic_subscribes",
                schema: "query",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    topic_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_topic_subscribes", x => new { x.user_id, x.topic_id });
                    table.ForeignKey(
                        name: "fk_topic_subscribes_topics_topic_id",
                        column: x => x.topic_id,
                        principalSchema: "query",
                        principalTable: "topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "column_images",
                schema: "query",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    thumbnail_url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_column_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_column_images_columns_id",
                        column: x => x.id,
                        principalSchema: "query",
                        principalTable: "columns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_columns_topic_id",
                schema: "query",
                table: "columns",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "ix_topic_subscribes_topic_id",
                schema: "query",
                table: "topic_subscribes",
                column: "topic_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "column_images",
                schema: "query");

            migrationBuilder.DropTable(
                name: "topic_subscribes",
                schema: "query");

            migrationBuilder.DropTable(
                name: "columns",
                schema: "query");

            migrationBuilder.DropTable(
                name: "topics",
                schema: "query");
        }
    }
}
