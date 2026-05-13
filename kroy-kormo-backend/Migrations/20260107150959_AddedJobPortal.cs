using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sob_Need_Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedJobPortal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationStatuses",
                columns: table => new
                {
                    ApplicationStatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    StatusName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Slug = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ColorCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsTerminal = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationStatuses", x => x.ApplicationStatusId);
                });

            migrationBuilder.CreateTable(
                name: "CompanyProfiles",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    Industry = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CompanySize = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LogoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CoverImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VerifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LinkedInUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    FacebookUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TwitterUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyProfiles", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_CompanyProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExperienceLevels",
                columns: table => new
                {
                    ExperienceLevelId = table.Column<Guid>(type: "uuid", nullable: false),
                    LevelName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Slug = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MinYears = table.Column<int>(type: "integer", nullable: false),
                    MaxYears = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceLevels", x => x.ExperienceLevelId);
                });

            migrationBuilder.CreateTable(
                name: "JobTypes",
                columns: table => new
                {
                    JobTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    TypeName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Slug = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTypes", x => x.JobTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    SkillId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.SkillId);
                });

            migrationBuilder.CreateTable(
                name: "UserEducations",
                columns: table => new
                {
                    EducationId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Degree = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FieldOfStudy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Institution = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsCurrent = table.Column<bool>(type: "boolean", nullable: false),
                    Grade = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Achievements = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEducations", x => x.EducationId);
                    table.ForeignKey(
                        name: "FK_UserEducations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserExperiences",
                columns: table => new
                {
                    ExperienceId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    JobTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EmploymentType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsCurrent = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    KeyAchievements = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Skills = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExperiences", x => x.ExperienceId);
                    table.ForeignKey(
                        name: "FK_UserExperiences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserResumes",
                columns: table => new
                {
                    ResumeId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FileUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ResumeTitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ParsedContent = table.Column<string>(type: "text", nullable: true),
                    TimesUsed = table.Column<int>(type: "integer", nullable: false),
                    LastUsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsScanned = table.Column<bool>(type: "boolean", nullable: false),
                    ScannedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserResumes", x => x.ResumeId);
                    table.ForeignKey(
                        name: "FK_UserResumes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobAlerts",
                columns: table => new
                {
                    AlertId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AlertName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Keywords = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    JobTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExperienceLevelId = table.Column<Guid>(type: "uuid", nullable: true),
                    MinSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    MaxSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    IsRemote = table.Column<bool>(type: "boolean", nullable: true),
                    IsHybrid = table.Column<bool>(type: "boolean", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    Frequency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    EmailNotification = table.Column<bool>(type: "boolean", nullable: false),
                    PushNotification = table.Column<bool>(type: "boolean", nullable: false),
                    LastSentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MatchCount = table.Column<int>(type: "integer", nullable: false),
                    LastMatchedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobAlerts", x => x.AlertId);
                    table.ForeignKey(
                        name: "FK_JobAlerts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_JobAlerts_ExperienceLevels_ExperienceLevelId",
                        column: x => x.ExperienceLevelId,
                        principalTable: "ExperienceLevels",
                        principalColumn: "ExperienceLevelId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_JobAlerts_JobTypes_JobTypeId",
                        column: x => x.JobTypeId,
                        principalTable: "JobTypes",
                        principalColumn: "JobTypeId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_JobAlerts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobListings",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyProfileId = table.Column<Guid>(type: "uuid", nullable: true),
                    JobTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExperienceLevelId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    SalaryMin = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    SalaryMax = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    SalaryCurrency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    SalaryPeriod = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ShowSalary = table.Column<bool>(type: "boolean", nullable: false),
                    VacancyCount = table.Column<int>(type: "integer", nullable: false),
                    ApplicationDeadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Requirements = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    Responsibilities = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    Benefits = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
                    PreferredQualifications = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IsRemote = table.Column<bool>(type: "boolean", nullable: false),
                    IsHybrid = table.Column<bool>(type: "boolean", nullable: false),
                    WorkSchedule = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    OfficeLocation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RequireResume = table.Column<bool>(type: "boolean", nullable: false),
                    RequireCoverLetter = table.Column<bool>(type: "boolean", nullable: false),
                    ApplicationCount = table.Column<int>(type: "integer", nullable: false),
                    ViewCount = table.Column<int>(type: "integer", nullable: false),
                    JobStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FilledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobListings", x => x.JobId);
                    table.ForeignKey(
                        name: "FK_JobListings_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobListings_CompanyProfiles_CompanyProfileId",
                        column: x => x.CompanyProfileId,
                        principalTable: "CompanyProfiles",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_JobListings_ExperienceLevels_ExperienceLevelId",
                        column: x => x.ExperienceLevelId,
                        principalTable: "ExperienceLevels",
                        principalColumn: "ExperienceLevelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobListings_JobTypes_JobTypeId",
                        column: x => x.JobTypeId,
                        principalTable: "JobTypes",
                        principalColumn: "JobTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobListings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobListings_Users_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSkills",
                columns: table => new
                {
                    UserSkillId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillId = table.Column<Guid>(type: "uuid", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "integer", nullable: true),
                    ProficiencyLevel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EndorsementCount = table.Column<int>(type: "integer", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VerificationSource = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkills", x => x.UserSkillId);
                    table.ForeignKey(
                        name: "FK_UserSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkills_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobApplications",
                columns: table => new
                {
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicantId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationStatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    CoverLetter = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    ResumeUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AttachmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IsShortlisted = table.Column<bool>(type: "boolean", nullable: false),
                    IsRejected = table.Column<bool>(type: "boolean", nullable: false),
                    EmployerNotes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReviewedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CandidateRating = table.Column<int>(type: "integer", nullable: true),
                    HiddenFromEmployers = table.Column<string>(type: "text", nullable: true),
                    ApplicationSource = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsWithdrawn = table.Column<bool>(type: "boolean", nullable: false),
                    WithdrawnAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WithdrawalReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplications", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK_JobApplications_ApplicationStatuses_ApplicationStatusId",
                        column: x => x.ApplicationStatusId,
                        principalTable: "ApplicationStatuses",
                        principalColumn: "ApplicationStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobApplications_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "AttachmentId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_JobApplications_JobListings_JobId",
                        column: x => x.JobId,
                        principalTable: "JobListings",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobApplications_Users_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobSkills",
                columns: table => new
                {
                    JobSkillId = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    MinYearsExperience = table.Column<int>(type: "integer", nullable: true),
                    ProficiencyLevel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSkills", x => x.JobSkillId);
                    table.ForeignKey(
                        name: "FK_JobSkills_JobListings_JobId",
                        column: x => x.JobId,
                        principalTable: "JobListings",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobViews",
                columns: table => new
                {
                    ViewId = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    IpAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ReferrerUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DeviceType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ViewDurationSeconds = table.Column<int>(type: "integer", nullable: true),
                    ViewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobViews", x => x.ViewId);
                    table.ForeignKey(
                        name: "FK_JobViews_JobListings_JobId",
                        column: x => x.JobId,
                        principalTable: "JobListings",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobViews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SavedJobs",
                columns: table => new
                {
                    SavedJobId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SavedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedJobs", x => x.SavedJobId);
                    table.ForeignKey(
                        name: "FK_SavedJobs_JobListings_JobId",
                        column: x => x.JobId,
                        principalTable: "JobListings",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedJobs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationStatusHistories",
                columns: table => new
                {
                    HistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ChangedByRole = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationStatusHistories", x => x.HistoryId);
                    table.ForeignKey(
                        name: "FK_ApplicationStatusHistories_ApplicationStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ApplicationStatuses",
                        principalColumn: "ApplicationStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationStatusHistories_JobApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "JobApplications",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Interviews",
                columns: table => new
                {
                    InterviewId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateId = table.Column<Guid>(type: "uuid", nullable: false),
                    InterviewType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MeetingLink = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    InterviewerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    InterviewerEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    InterviewerPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Feedback = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
                    Rating = table.Column<int>(type: "integer", nullable: true),
                    CandidateConfirmed = table.Column<bool>(type: "boolean", nullable: true),
                    ConfirmedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CandidateNotes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CancelledBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancellationReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RescheduledFrom = table.Column<Guid>(type: "uuid", nullable: true),
                    RescheduledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interviews", x => x.InterviewId);
                    table.ForeignKey(
                        name: "FK_Interviews_JobApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "JobApplications",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Interviews_Users_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Interviews_Users_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobOffers",
                columns: table => new
                {
                    OfferId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateId = table.Column<Guid>(type: "uuid", nullable: false),
                    SalaryOffered = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SalaryCurrency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SalaryPeriod = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BonusAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    BonusDescription = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    JobTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Department = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EmploymentType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Benefits = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
                    Terms = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    ProbationPeriodMonths = table.Column<int>(type: "integer", nullable: true),
                    NoticePeriodDays = table.Column<int>(type: "integer", nullable: true),
                    WorkLocation = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsRemote = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CandidateResponse = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    RespondedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CounterOfferSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CounterOfferNotes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AcceptedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RejectionReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ContractAttachmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    ContractSigned = table.Column<bool>(type: "boolean", nullable: false),
                    ContractSignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOffers", x => x.OfferId);
                    table.ForeignKey(
                        name: "FK_JobOffers_JobApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "JobApplications",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobOffers_Users_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobOffers_Users_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AdminUsers",
                keyColumn: "AdminId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 7, 15, 9, 59, 264, DateTimeKind.Utc).AddTicks(2634));

            migrationBuilder.InsertData(
                table: "ApplicationStatuses",
                columns: new[] { "ApplicationStatusId", "ColorCode", "CreatedAt", "CreatedBy", "Description", "IsActive", "IsTerminal", "Slug", "StatusName", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("cc111111-1111-1111-1111-111111111111"), "#3498db", new DateTime(2026, 1, 7, 15, 9, 59, 264, DateTimeKind.Utc).AddTicks(2820), null, null, true, false, "applied", "Applied", null, null },
                    { new Guid("cc222222-2222-2222-2222-222222222222"), "#f1c40f", new DateTime(2026, 1, 7, 15, 9, 59, 264, DateTimeKind.Utc).AddTicks(2823), null, null, true, false, "shortlisted", "Shortlisted", null, null },
                    { new Guid("cc333333-3333-3333-3333-333333333333"), "#9b59b6", new DateTime(2026, 1, 7, 15, 9, 59, 264, DateTimeKind.Utc).AddTicks(2825), null, null, true, false, "interviewing", "Interviewing", null, null },
                    { new Guid("cc444444-4444-4444-4444-444444444444"), "#2ecc71", new DateTime(2026, 1, 7, 15, 9, 59, 264, DateTimeKind.Utc).AddTicks(2866), null, null, true, false, "offered", "Offered", null, null },
                    { new Guid("cc555555-5555-5555-5555-555555555555"), "#e74c3c", new DateTime(2026, 1, 7, 15, 9, 59, 264, DateTimeKind.Utc).AddTicks(2868), null, null, true, true, "rejected", "Rejected", null, null },
                    { new Guid("cc666666-6666-6666-6666-666666666666"), "#27ae60", new DateTime(2026, 1, 7, 15, 9, 59, 264, DateTimeKind.Utc).AddTicks(2870), null, null, true, true, "hired", "Hired", null, null },
                    { new Guid("cc777777-7777-7777-7777-777777777777"), "#95a5a6", new DateTime(2026, 1, 7, 15, 9, 59, 264, DateTimeKind.Utc).AddTicks(2873), null, null, true, true, "withdrawn", "Withdrawn", null, null }
                });


            migrationBuilder.InsertData(
                table: "ExperienceLevels",
                columns: new[] { "ExperienceLevelId", "CreatedAt", "CreatedBy", "Description", "IsActive", "LevelName", "MaxYears", "MinYears", "Slug", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("bb111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 7, 15, 9, 59, 264, DateTimeKind.Utc).AddTicks(2792), null, null, true, "Entry Level", 2, 0, "entry", null, null },
                    { new Guid("bb222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 7, 15, 9, 59, 264, DateTimeKind.Utc).AddTicks(2795), null, null, true, "Mid Level", 5, 2, "mid", null, null },
                    { new Guid("bb333333-3333-3333-3333-333333333333"), new DateTime(2026, 1, 7, 15, 9, 59, 264, DateTimeKind.Utc).AddTicks(2797), null, null, true, "Senior Level", 10, 5, "senior", null, null },
                    { new Guid("bb444444-4444-4444-4444-444444444444"), new DateTime(2026, 1, 7, 15, 9, 59, 264, DateTimeKind.Utc).AddTicks(2800), null, null, true, "Executive", null, 10, "executive", null, null }
                });

            migrationBuilder.InsertData(
                table: "JobTypes",
                columns: new[] { "JobTypeId", "CreatedAt", "CreatedBy", "Description", "IsActive", "Slug", "TypeName", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aa111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 7, 15, 9, 59, 264, DateTimeKind.Utc).AddTicks(2747), null, null, true, "full-time", "Full-time", null, null },
                    { new Guid("aa222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 7, 15, 9, 59, 264, DateTimeKind.Utc).AddTicks(2758), null, null, true, "part-time", "Part-time", null, null },
                    { new Guid("aa333333-3333-3333-3333-333333333333"), new DateTime(2026, 1, 7, 15, 9, 59, 264, DateTimeKind.Utc).AddTicks(2763), null, null, true, "contract", "Contract", null, null },
                    { new Guid("aa444444-4444-4444-4444-444444444444"), new DateTime(2026, 1, 7, 15, 9, 59, 264, DateTimeKind.Utc).AddTicks(2765), null, null, true, "internship", "Internship", null, null },
                    { new Guid("aa555555-5555-5555-5555-555555555555"), new DateTime(2026, 1, 7, 15, 9, 59, 264, DateTimeKind.Utc).AddTicks(2767), null, null, true, "remote", "Remote", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationStatusHistories_ApplicationId",
                table: "ApplicationStatusHistories",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationStatusHistories_StatusId",
                table: "ApplicationStatusHistories",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_Slug",
                table: "CompanyProfiles",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_Verified_Active",
                table: "CompanyProfiles",
                columns: new[] { "IsVerified", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfiles_UserId",
                table: "CompanyProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Interview_Candidate_Schedule_Status",
                table: "Interviews",
                columns: new[] { "CandidateId", "ScheduledAt", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Interview_Employer_Schedule_Status",
                table: "Interviews",
                columns: new[] { "EmployerId", "ScheduledAt", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_ApplicationId",
                table: "Interviews",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAlerts_CategoryId",
                table: "JobAlerts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAlerts_ExperienceLevelId",
                table: "JobAlerts",
                column: "ExperienceLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAlerts_JobTypeId",
                table: "JobAlerts",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAlerts_UserId",
                table: "JobAlerts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplication_Applicant_Status_Active",
                table: "JobApplications",
                columns: new[] { "ApplicantId", "ApplicationStatusId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_JobApplication_Job_Applicant_Unique",
                table: "JobApplications",
                columns: new[] { "JobId", "ApplicantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobApplication_Job_Status_Date",
                table: "JobApplications",
                columns: new[] { "JobId", "ApplicationStatusId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_ApplicationStatusId",
                table: "JobApplications",
                column: "ApplicationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_AttachmentId",
                table: "JobApplications",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_JobListing_Category_Active_Date",
                table: "JobListings",
                columns: new[] { "CategoryId", "IsActive", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_JobListing_Deadline",
                table: "JobListings",
                column: "ApplicationDeadline");

            migrationBuilder.CreateIndex(
                name: "IX_JobListing_Employer_Status_Active",
                table: "JobListings",
                columns: new[] { "EmployerId", "JobStatus", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_JobListing_Type_Level_Remote_Active",
                table: "JobListings",
                columns: new[] { "JobTypeId", "ExperienceLevelId", "IsRemote", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_JobListings_CompanyProfileId",
                table: "JobListings",
                column: "CompanyProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_JobListings_ExperienceLevelId",
                table: "JobListings",
                column: "ExperienceLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_JobListings_ProductId",
                table: "JobListings",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_ApplicationId",
                table: "JobOffers",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_CandidateId",
                table: "JobOffers",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_EmployerId",
                table: "JobOffers",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSkills_JobId",
                table: "JobSkills",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSkills_SkillId",
                table: "JobSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_JobView_Job_Date",
                table: "JobViews",
                columns: new[] { "JobId", "ViewedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_JobView_User_Date",
                table: "JobViews",
                columns: new[] { "UserId", "ViewedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_SavedJob_User_Job_Unique",
                table: "SavedJobs",
                columns: new[] { "UserId", "JobId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavedJobs_JobId",
                table: "SavedJobs",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_Name",
                table: "Skills",
                column: "SkillName");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_Slug",
                table: "Skills",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserEducations_UserId",
                table: "UserEducations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExperiences_UserId",
                table: "UserExperiences",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserResumes_UserId",
                table: "UserResumes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_SkillId",
                table: "UserSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_UserId",
                table: "UserSkills",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationStatusHistories");

            migrationBuilder.DropTable(
                name: "Interviews");

            migrationBuilder.DropTable(
                name: "JobAlerts");

            migrationBuilder.DropTable(
                name: "JobOffers");

            migrationBuilder.DropTable(
                name: "JobSkills");

            migrationBuilder.DropTable(
                name: "JobViews");

            migrationBuilder.DropTable(
                name: "SavedJobs");

            migrationBuilder.DropTable(
                name: "UserEducations");

            migrationBuilder.DropTable(
                name: "UserExperiences");

            migrationBuilder.DropTable(
                name: "UserResumes");

            migrationBuilder.DropTable(
                name: "UserSkills");

            migrationBuilder.DropTable(
                name: "JobApplications");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "ApplicationStatuses");

            migrationBuilder.DropTable(
                name: "JobListings");

            migrationBuilder.DropTable(
                name: "CompanyProfiles");

            migrationBuilder.DropTable(
                name: "ExperienceLevels");

            migrationBuilder.DropTable(
                name: "JobTypes");


            migrationBuilder.UpdateData(
                table: "AdminUsers",
                keyColumn: "AdminId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 6, 10, 54, 46, 823, DateTimeKind.Utc).AddTicks(9849));
        }
    }
}
