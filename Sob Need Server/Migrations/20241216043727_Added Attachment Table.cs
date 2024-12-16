using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sob_Need_Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedAttachmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"), // Use "Npgsql:ValueGenerationStrategy" for PostgreSQL
                    ProductId = table.Column<int>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: false),
                    AltName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.AttachmentId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments");
        }
    }
}
