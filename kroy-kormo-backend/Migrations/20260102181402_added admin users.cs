using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sob_Need_Server.Migrations
{
    /// <inheritdoc />
    public partial class addedadminusers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    AdminId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Role = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.AdminId);
                });

            migrationBuilder.InsertData(
                table: "AdminUsers",
                columns: new[] { "AdminId", "CreatedAt", "Email", "FullName", "IsActive", "LastLoginAt", "Password", "Phone", "Role", "UserName" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 2, 18, 14, 1, 913, DateTimeKind.Utc).AddTicks(1216), "admin@kroykormo.com", "Super Admin", true, null, "$2a$11$0KjJ5q.ZgY.Qx.Qx.Qx.Qx.Qx.Qx.Qx.Qx.Qx.Qx.Qx.Qx.Qx", null, "SuperAdmin", "superadmin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");
        }
    }
}
