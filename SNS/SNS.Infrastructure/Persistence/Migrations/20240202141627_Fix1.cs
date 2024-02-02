using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SNS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_subscriber_albums_album_id",
                table: "subscriber");

            migrationBuilder.DropForeignKey(
                name: "fk_subscriber_users_subscriber_id",
                table: "subscriber");

            migrationBuilder.DropPrimaryKey(
                name: "pk_subscriber",
                table: "subscriber");

            migrationBuilder.RenameTable(
                name: "subscriber",
                newName: "subscribers");

            migrationBuilder.RenameIndex(
                name: "ix_subscriber_album_id",
                table: "subscribers",
                newName: "ix_subscribers_album_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_subscribers",
                table: "subscribers",
                column: "subscriber_id");

            migrationBuilder.AddForeignKey(
                name: "fk_subscribers_albums_album_id",
                table: "subscribers",
                column: "album_id",
                principalTable: "albums",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_subscribers_users_subscriber_id",
                table: "subscribers",
                column: "subscriber_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_subscribers_albums_album_id",
                table: "subscribers");

            migrationBuilder.DropForeignKey(
                name: "fk_subscribers_users_subscriber_id",
                table: "subscribers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_subscribers",
                table: "subscribers");

            migrationBuilder.RenameTable(
                name: "subscribers",
                newName: "subscriber");

            migrationBuilder.RenameIndex(
                name: "ix_subscribers_album_id",
                table: "subscriber",
                newName: "ix_subscriber_album_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_subscriber",
                table: "subscriber",
                column: "subscriber_id");

            migrationBuilder.AddForeignKey(
                name: "fk_subscriber_albums_album_id",
                table: "subscriber",
                column: "album_id",
                principalTable: "albums",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_subscriber_users_subscriber_id",
                table: "subscriber",
                column: "subscriber_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
