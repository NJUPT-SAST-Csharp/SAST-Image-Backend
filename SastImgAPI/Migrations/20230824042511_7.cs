using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SastImgAPI.Migrations
{
    /// <inheritdoc />
    public partial class _7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_images_classifications_category_id",
                table: "images");

            migrationBuilder.DropPrimaryKey(
                name: "pk_classifications",
                table: "classifications");

            migrationBuilder.RenameTable(
                name: "classifications",
                newName: "categories");

            migrationBuilder.RenameIndex(
                name: "ix_classifications_name",
                table: "categories",
                newName: "ix_categories_name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_categories",
                table: "categories",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_images_categories_category_id",
                table: "images",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_images_categories_category_id",
                table: "images");

            migrationBuilder.DropPrimaryKey(
                name: "pk_categories",
                table: "categories");

            migrationBuilder.RenameTable(
                name: "categories",
                newName: "classifications");

            migrationBuilder.RenameIndex(
                name: "ix_categories_name",
                table: "classifications",
                newName: "ix_classifications_name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_classifications",
                table: "classifications",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_images_classifications_category_id",
                table: "images",
                column: "category_id",
                principalTable: "classifications",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
