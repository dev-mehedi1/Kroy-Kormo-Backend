using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sob_Need_Server.Migrations
{
    /// <inheritdoc />
    public partial class addextrapropartyinproductmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discount",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeliveryFree",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeaturedMember",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsJobAd",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTopAd",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUrgentAd",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDeliveryFree",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsFeaturedMember",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsJobAd",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsTopAd",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsUrgentAd",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Products");
        }
    }
}
