using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Account.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class ProfileRefactor : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "profiles");

        migrationBuilder.AddColumn<string>(
            name: "avatar",
            table: "users",
            type: "text",
            nullable: true
        );

        migrationBuilder.AddColumn<string>(
            name: "biography",
            table: "users",
            type: "text",
            nullable: true
        );

        migrationBuilder.AddColumn<DateOnly>(
            name: "birthday",
            table: "users",
            type: "date",
            nullable: true
        );

        migrationBuilder.AddColumn<string>(
            name: "header",
            table: "users",
            type: "text",
            nullable: true
        );

        migrationBuilder.AddColumn<string>(
            name: "nickname",
            table: "users",
            type: "text",
            nullable: true
        );

        migrationBuilder.AddColumn<string>(
            name: "website",
            table: "users",
            type: "text",
            nullable: true
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "avatar", table: "users");

        migrationBuilder.DropColumn(name: "biography", table: "users");

        migrationBuilder.DropColumn(name: "birthday", table: "users");

        migrationBuilder.DropColumn(name: "header", table: "users");

        migrationBuilder.DropColumn(name: "nickname", table: "users");

        migrationBuilder.DropColumn(name: "website", table: "users");

        migrationBuilder.CreateTable(
            name: "profiles",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false),
                avatar = table.Column<string>(type: "text", nullable: true),
                biography = table.Column<string>(type: "text", nullable: false),
                header = table.Column<string>(type: "text", nullable: true),
                nickname = table.Column<string>(type: "text", nullable: false),
                website = table.Column<string>(type: "text", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_profiles", x => x.id);
                table.ForeignKey(
                    name: "fk_profiles_users_id",
                    column: x => x.id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );
    }
}
