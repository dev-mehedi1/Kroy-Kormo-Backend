using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sob_Need_Server.Migrations
{
    /// <inheritdoc />
    public partial class AddJobApplicationEnhancementFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboutMe",
                table: "Users",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentSalary",
                table: "Users",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExpectedSalary",
                table: "Users",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFresher",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSalaryVisible",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotalExperienceMonths",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileId",
                table: "UserResumes",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AboutMeSnapshot",
                table: "JobApplications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentSalary",
                table: "JobApplications",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExpectedSalary",
                table: "JobApplications",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFresher",
                table: "JobApplications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotalExperienceMonths",
                table: "JobApplications",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutMe",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrentSalary",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ExpectedSalary",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsFresher",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsSalaryVisible",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotalExperienceMonths",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "UserResumes");

            migrationBuilder.DropColumn(
                name: "AboutMeSnapshot",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "CurrentSalary",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "ExpectedSalary",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "IsFresher",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "TotalExperienceMonths",
                table: "JobApplications");
        }
    }
}
