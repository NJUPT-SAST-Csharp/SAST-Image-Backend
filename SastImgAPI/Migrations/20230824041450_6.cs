using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SastImgAPI.Migrations
{
    /// <inheritdoc />
    public partial class _6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_images_classifications_classification_id",
                table: "images");

            migrationBuilder.DropForeignKey(
                name: "fk_images_tags_tag_id",
                table: "images");

            migrationBuilder.DropIndex(
                name: "ix_images_tag_id",
                table: "images");

            migrationBuilder.DropColumn(
                name: "tag_id",
                table: "images");

            migrationBuilder.RenameColumn(
                name: "classification_id",
                table: "images",
                newName: "category_id");

            migrationBuilder.RenameIndex(
                name: "ix_images_classification_id",
                table: "images",
                newName: "ix_images_category_id");

            migrationBuilder.CreateTable(
                name: "image_tag",
                columns: table => new
                {
                    images_id = table.Column<int>(type: "integer", nullable: false),
                    tags_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_image_tag", x => new { x.images_id, x.tags_id });
                    table.ForeignKey(
                        name: "fk_image_tag_images_images_id",
                        column: x => x.images_id,
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_image_tag_tags_tags_id",
                        column: x => x.tags_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_image_tag_tags_id",
                table: "image_tag",
                column: "tags_id");

            migrationBuilder.AddForeignKey(
                name: "fk_images_classifications_category_id",
                table: "images",
                column: "category_id",
                principalTable: "classifications",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_images_classifications_category_id",
                table: "images");

            migrationBuilder.DropTable(
                name: "image_tag");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "images",
                newName: "classification_id");

            migrationBuilder.RenameIndex(
                name: "ix_images_category_id",
                table: "images",
                newName: "ix_images_classification_id");

            migrationBuilder.AddColumn<int>(
                name: "tag_id",
                table: "images",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_images_tag_id",
                table: "images",
                column: "tag_id");

            migrationBuilder.AddForeignKey(
                name: "fk_images_classifications_classification_id",
                table: "images",
                column: "classification_id",
                principalTable: "classifications",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_images_tags_tag_id",
                table: "images",
                column: "tag_id",
                principalTable: "tags",
                principalColumn: "id");
        }
    }
}
