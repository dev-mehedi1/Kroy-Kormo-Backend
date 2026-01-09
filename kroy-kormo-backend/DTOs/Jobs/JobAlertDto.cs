using System;

namespace Sob_Need_Server.DTOs.Jobs
{
    public class JobAlertDto
    {
        public Guid AlertId { get; set; }
        public Guid UserId { get; set; }
        public string AlertName { get; set; } = null!;
        public string? Keywords { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? JobTypeId { get; set; }
        public string? JobTypeName { get; set; }
        public Guid? ExperienceLevelId { get; set; }
        public string? ExperienceLevelName { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public bool? IsRemote { get; set; }
        public bool? IsHybrid { get; set; }
        public Guid? CompanyId { get; set; }
        public string Frequency { get; set; } = "Daily";
        public bool IsActive { get; set; }
        public bool EmailNotification { get; set; }
        public bool PushNotification { get; set; }
        public int MatchCount { get; set; }
        public DateTime? LastSentAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateJobAlertDto
    {
        public string AlertName { get; set; } = null!;
        public string? Keywords { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? JobTypeId { get; set; }
        public Guid? ExperienceLevelId { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public bool? IsRemote { get; set; }
        public bool? IsHybrid { get; set; }
        public Guid? CompanyId { get; set; }
        public string Frequency { get; set; } = "Daily";
        public bool EmailNotification { get; set; } = true;
        public bool PushNotification { get; set; } = true;
    }

    public class UpdateJobAlertDto : CreateJobAlertDto
    {
        public bool IsActive { get; set; }
    }
}
