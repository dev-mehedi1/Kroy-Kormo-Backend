using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sob_Need_Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGoogleDriveIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileId",
                table: "UserResumes");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "UserResumes");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "UserResumes");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "UserResumes");

            migrationBuilder.DropColumn(
                name: "IsScanned",
                table: "UserResumes");

            migrationBuilder.DropColumn(
                name: "ScannedAt",
                table: "UserResumes");

            migrationBuilder.RenameColumn(
                name: "FileUrl",
                table: "UserResumes",
                newName: "ResumeUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResumeUrl",
                table: "UserResumes",
                newName: "FileUrl");

            migrationBuilder.AddColumn<string>(
                name: "FileId",
                table: "UserResumes",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "UserResumes",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "UserResumes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "UserResumes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsScanned",
                table: "UserResumes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScannedAt",
                table: "UserResumes",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
