using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sob_Need_Server.Migrations
{
    /// <inheritdoc />
    public partial class addedproductstatushistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductStatusHistories",
                columns: table => new
                {
                    HistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ChangedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProductId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStatusHistories", x => x.HistoryId);
                    table.ForeignKey(
                        name: "FK_ProductStatusHistories_ProductStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ProductStatuses",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductStatusHistories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductStatusHistories_Products_ProductId1",
                        column: x => x.ProductId1,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductStatusHistories_ProductId",
                table: "ProductStatusHistories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStatusHistories_ProductId1",
                table: "ProductStatusHistories",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStatusHistories_StatusId",
                table: "ProductStatusHistories",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductStatusHistories");
        }
    }
}
