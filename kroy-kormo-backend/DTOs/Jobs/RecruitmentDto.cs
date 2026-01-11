namespace Sob_Need_Server.DTOs.Jobs
{
    public class CompanyProfileDto
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? Industry { get; set; }
        public string? CompanySize { get; set; }
        public string? Website { get; set; }
        public string? LogoUrl { get; set; }
        public string? CoverImageUrl { get; set; }
        public bool IsVerified { get; set; }
    }

    public class InterviewDto
    {
        public Guid InterviewId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid JobId { get; set; }
        public string JobTitle { get; set; } = null!;
        public string? CompanyName { get; set; }
        public string? CompanyLogo { get; set; }
        public string CandidateName { get; set; } = null!;
        
        public string InterviewType { get; set; } = null!;
        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; }
        public string? Location { get; set; }
        public string? MeetingLink { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = null!;
        
        // Post-Interview Management
        public string? Feedback { get; set; }
        public string? Outcome { get; set; } // "Pending", "Accepted", "Rejected"
        public DateTime? CompletedAt { get; set; }
        public string? RejectionReason { get; set; }
    }

    public class InterviewFeedbackDto
    {
        public Guid InterviewId { get; set; }
        public string Feedback { get; set; } = null!;
        public string Outcome { get; set; } = null!; // "Accepted" or "Rejected"
        public string? RejectionReason { get; set; }
    }

    public class JobOfferDto
    {
        public Guid OfferId { get; set; }
        public Guid ApplicationId { get; set; }
        public string JobTitle { get; set; } = null!;
        
        public decimal SalaryOffered { get; set; }
        public string SalaryCurrency { get; set; } = null!;
        public string SalaryPeriod { get; set; } = null!;
        
        public DateTime? StartDate { get; set; }
        public string? Benefits { get; set; }
        public string? Terms { get; set; }
        public string Status { get; set; } = null!;
    }

    public class JobStatisticsDto
    {
        public int ViewCount { get; set; }
        public int ApplicationCount { get; set; }
        public int ShortlistedCount { get; set; }
        public int InterviewCount { get; set; }
        public Dictionary<string, int> ApplicationsByStatus { get; set; } = new();
    }
}
