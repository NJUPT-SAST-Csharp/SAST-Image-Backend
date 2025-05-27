using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Account.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class Fix1 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(name: "salt", table: "users", newName: "password_salt");

        migrationBuilder.RenameColumn(name: "hash", table: "users", newName: "password_hash");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(name: "password_salt", table: "users", newName: "salt");

        migrationBuilder.RenameColumn(name: "password_hash", table: "users", newName: "hash");
    }
}
