using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SastImgAPI.Migrations
{
    /// <inheritdoc />
    public partial class _0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table =>
                    new
                    {
                        id = table
                            .Column<long>(type: "bigint", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        name = table.Column<string>(
                            type: "character varying(256)",
                            maxLength: 256,
                            nullable: true
                        ),
                        normalized_name = table.Column<string>(
                            type: "character varying(256)",
                            maxLength: 256,
                            nullable: true
                        ),
                        concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_roles", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table =>
                    new
                    {
                        id = table
                            .Column<long>(type: "bigint", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        nickname = table.Column<string>(type: "text", nullable: false),
                        biography = table.Column<string>(type: "text", nullable: false),
                        avatar = table.Column<string>(type: "text", nullable: true),
                        header = table.Column<string>(type: "text", nullable: true),
                        registered_at = table.Column<DateTime>(
                            type: "timestamp with time zone",
                            nullable: false
                        ),
                        user_name = table.Column<string>(
                            type: "character varying(256)",
                            maxLength: 256,
                            nullable: true
                        ),
                        normalized_user_name = table.Column<string>(
                            type: "character varying(256)",
                            maxLength: 256,
                            nullable: true
                        ),
                        email = table.Column<string>(
                            type: "character varying(256)",
                            maxLength: 256,
                            nullable: true
                        ),
                        normalized_email = table.Column<string>(
                            type: "character varying(256)",
                            maxLength: 256,
                            nullable: true
                        ),
                        email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                        password_hash = table.Column<string>(type: "text", nullable: true),
                        security_stamp = table.Column<string>(type: "text", nullable: true),
                        concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                        phone_number = table.Column<string>(type: "text", nullable: true),
                        phone_number_confirmed = table.Column<bool>(
                            type: "boolean",
                            nullable: false
                        ),
                        two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                        lockout_end = table.Column<DateTimeOffset>(
                            type: "timestamp with time zone",
                            nullable: true
                        ),
                        lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                        access_failed_count = table.Column<int>(type: "integer", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_users", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table =>
                    new
                    {
                        id = table
                            .Column<long>(type: "bigint", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        name = table.Column<string>(type: "text", nullable: false),
                        description = table.Column<string>(type: "text", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table =>
                    new
                    {
                        id = table
                            .Column<long>(type: "bigint", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        name = table.Column<string>(type: "text", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tags", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table =>
                    new
                    {
                        id = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        role_id = table.Column<long>(type: "bigint", nullable: false),
                        claim_type = table.Column<string>(type: "text", nullable: true),
                        claim_value = table.Column<string>(type: "text", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "albums",
                columns: table =>
                    new
                    {
                        id = table
                            .Column<long>(type: "bigint", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        name = table.Column<string>(type: "text", nullable: false),
                        description = table.Column<string>(type: "text", nullable: false),
                        cover = table.Column<string>(type: "text", nullable: true),
                        author_id = table.Column<long>(type: "bigint", nullable: false),
                        created_at = table.Column<DateTime>(
                            type: "timestamp with time zone",
                            nullable: false
                        ),
                        accessibility = table.Column<int>(type: "integer", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("pk_albums", x => x.id);
                    table.ForeignKey(
                        name: "fk_albums_users_author_id",
                        column: x => x.author_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table =>
                    new
                    {
                        id = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        user_id = table.Column<long>(type: "bigint", nullable: false),
                        claim_type = table.Column<string>(type: "text", nullable: true),
                        claim_value = table.Column<string>(type: "text", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_user_claims_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table =>
                    new
                    {
                        login_provider = table.Column<string>(type: "text", nullable: false),
                        provider_key = table.Column<string>(type: "text", nullable: false),
                        provider_display_name = table.Column<string>(type: "text", nullable: true),
                        user_id = table.Column<long>(type: "bigint", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "pk_asp_net_user_logins",
                        x => new { x.login_provider, x.provider_key }
                    );
                    table.ForeignKey(
                        name: "fk_asp_net_user_logins_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table =>
                    new
                    {
                        user_id = table.Column<long>(type: "bigint", nullable: false),
                        role_id = table.Column<long>(type: "bigint", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table =>
                    new
                    {
                        user_id = table.Column<long>(type: "bigint", nullable: false),
                        login_provider = table.Column<string>(type: "text", nullable: false),
                        name = table.Column<string>(type: "text", nullable: false),
                        value = table.Column<string>(type: "text", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "pk_asp_net_user_tokens",
                        x =>
                            new
                            {
                                x.user_id,
                                x.login_provider,
                                x.name
                            }
                    );
                    table.ForeignKey(
                        name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table =>
                    new
                    {
                        id = table
                            .Column<long>(type: "bigint", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        title = table.Column<string>(type: "text", nullable: false),
                        content = table.Column<string>(type: "text", nullable: false),
                        is_read = table.Column<bool>(type: "boolean", nullable: false),
                        user_id = table.Column<long>(type: "bigint", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_notifications_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "user_user",
                columns: table =>
                    new
                    {
                        followers_id = table.Column<long>(type: "bigint", nullable: false),
                        following_id = table.Column<long>(type: "bigint", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_user", x => new { x.followers_id, x.following_id });
                    table.ForeignKey(
                        name: "fk_user_user_users_followers_id",
                        column: x => x.followers_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_user_user_users_following_id",
                        column: x => x.following_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "images",
                columns: table =>
                    new
                    {
                        id = table
                            .Column<long>(type: "bigint", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        title = table.Column<string>(type: "text", nullable: false),
                        description = table.Column<string>(type: "text", nullable: false),
                        category_id = table.Column<long>(type: "bigint", nullable: false),
                        created_at = table.Column<DateTime>(
                            type: "timestamp with time zone",
                            nullable: false
                        ),
                        album_id = table.Column<long>(type: "bigint", nullable: false),
                        author_id = table.Column<long>(type: "bigint", nullable: false),
                        views = table.Column<int>(type: "integer", nullable: false),
                        is_exif_enabled = table.Column<bool>(type: "boolean", nullable: false),
                        url = table.Column<string>(type: "text", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("pk_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_images_albums_album_id",
                        column: x => x.album_id,
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_images_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_images_users_author_id",
                        column: x => x.author_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "image_tag",
                columns: table =>
                    new
                    {
                        images_id = table.Column<long>(type: "bigint", nullable: false),
                        tags_id = table.Column<long>(type: "bigint", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("pk_image_tag", x => new { x.images_id, x.tags_id });
                    table.ForeignKey(
                        name: "fk_image_tag_images_images_id",
                        column: x => x.images_id,
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_image_tag_tags_tags_id",
                        column: x => x.tags_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_albums_author_id",
                table: "albums",
                column: "author_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_role_claims_role_id",
                table: "AspNetRoleClaims",
                column: "role_id"
            );

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "normalized_name",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_claims_user_id",
                table: "AspNetUserClaims",
                column: "user_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_logins_user_id",
                table: "AspNetUserLogins",
                column: "user_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_roles_role_id",
                table: "AspNetUserRoles",
                column: "role_id"
            );

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "normalized_email"
            );

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "normalized_user_name",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_categories_name",
                table: "categories",
                column: "name",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_image_tag_tags_id",
                table: "image_tag",
                column: "tags_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_images_album_id",
                table: "images",
                column: "album_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_images_author_id",
                table: "images",
                column: "author_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_images_category_id",
                table: "images",
                column: "category_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_notifications_user_id",
                table: "notifications",
                column: "user_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_tags_name",
                table: "tags",
                column: "name",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_user_user_following_id",
                table: "user_user",
                column: "following_id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "AspNetRoleClaims");

            migrationBuilder.DropTable(name: "AspNetUserClaims");

            migrationBuilder.DropTable(name: "AspNetUserLogins");

            migrationBuilder.DropTable(name: "AspNetUserRoles");

            migrationBuilder.DropTable(name: "AspNetUserTokens");

            migrationBuilder.DropTable(name: "image_tag");

            migrationBuilder.DropTable(name: "notifications");

            migrationBuilder.DropTable(name: "user_user");

            migrationBuilder.DropTable(name: "AspNetRoles");

            migrationBuilder.DropTable(name: "images");

            migrationBuilder.DropTable(name: "tags");

            migrationBuilder.DropTable(name: "albums");

            migrationBuilder.DropTable(name: "categories");

            migrationBuilder.DropTable(name: "AspNetUsers");
        }
    }
}
