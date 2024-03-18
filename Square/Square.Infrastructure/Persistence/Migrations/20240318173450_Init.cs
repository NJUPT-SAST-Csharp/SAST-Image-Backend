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
            migrationBuilder.CreateTable(
                name: "topics",
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
                columns: table => new
                {
                    column_id = table.Column<long>(type: "bigint", nullable: false),
                    topic_id = table.Column<long>(type: "bigint", nullable: false),
                    author_id = table.Column<long>(type: "bigint", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_columns", x => x.column_id);
                    table.ForeignKey(
                        name: "fk_columns_topics_topic_id",
                        column: x => x.topic_id,
                        principalTable: "topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subscribers",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    topic_id = table.Column<long>(type: "bigint", nullable: false),
                    subscribed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscribers", x => new { x.user_id, x.topic_id });
                    table.ForeignKey(
                        name: "fk_subscribers_topics_topic_id",
                        column: x => x.topic_id,
                        principalTable: "topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "column_images",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    column_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_column_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_column_images_columns_column_id",
                        column: x => x.column_id,
                        principalTable: "columns",
                        principalColumn: "column_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "column_likes",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    column_id = table.Column<long>(type: "bigint", nullable: false),
                    liked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_column_likes", x => new { x.column_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_column_likes_columns_column_id",
                        column: x => x.column_id,
                        principalTable: "columns",
                        principalColumn: "column_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_column_images_column_id",
                table: "column_images",
                column: "column_id");

            migrationBuilder.CreateIndex(
                name: "ix_columns_topic_id",
                table: "columns",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "ix_subscribers_topic_id",
                table: "subscribers",
                column: "topic_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "column_images");

            migrationBuilder.DropTable(
                name: "column_likes");

            migrationBuilder.DropTable(
                name: "subscribers");

            migrationBuilder.DropTable(
                name: "columns");

            migrationBuilder.DropTable(
                name: "topics");
        }
    }
}
