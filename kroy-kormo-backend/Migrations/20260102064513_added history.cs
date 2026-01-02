using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sob_Need_Server.Migrations
{
    /// <inheritdoc />
    public partial class addedhistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductStatusHistories_Products_ProductId1",
                table: "ProductStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_ProductStatusHistories_ProductId1",
                table: "ProductStatusHistories");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "ProductStatusHistories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId1",
                table: "ProductStatusHistories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductStatusHistories_ProductId1",
                table: "ProductStatusHistories",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStatusHistories_Products_ProductId1",
                table: "ProductStatusHistories",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "ProductId");
        }
    }
}
