using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sob_Need_Server.Migrations
{
    /// <inheritdoc />
    public partial class updatethelocationtableforFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "States",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Regions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "Countries",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "States");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Countries");
        }
    }
}
