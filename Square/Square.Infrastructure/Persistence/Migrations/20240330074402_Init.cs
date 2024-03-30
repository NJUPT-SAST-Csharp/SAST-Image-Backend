using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Square.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "domain");

            migrationBuilder.CreateTable(
                name: "topics",
                schema: "domain",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    author_id = table.Column<long>(type: "bigint", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_topics", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "columns",
                schema: "domain",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    author_id = table.Column<long>(type: "bigint", nullable: false),
                    topic_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_columns", x => x.id);
                    table.ForeignKey(
                        name: "fk_columns_topics_topic_id",
                        column: x => x.topic_id,
                        principalSchema: "domain",
                        principalTable: "topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subscribers",
                schema: "domain",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    topic_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscribers", x => new { x.user_id, x.topic_id });
                    table.ForeignKey(
                        name: "fk_subscribers_topics_topic_id",
                        column: x => x.topic_id,
                        principalSchema: "domain",
                        principalTable: "topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "column_likes",
                schema: "domain",
                columns: table => new
                {
                    column_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_column_likes", x => x.column_id);
                    table.ForeignKey(
                        name: "fk_column_likes_columns_column_id",
                        column: x => x.column_id,
                        principalSchema: "domain",
                        principalTable: "columns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_columns__topic_id",
                schema: "domain",
                table: "columns",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "ix_subscribers_topic_id",
                schema: "domain",
                table: "subscribers",
                column: "topic_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "column_likes",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "subscribers",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "columns",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "topics",
                schema: "domain");
        }
    }
}
