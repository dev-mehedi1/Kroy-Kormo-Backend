namespace Sob_Need_Server.DTOs.Jobs
{
    public class JobApplicationDto
    {
        public Guid ApplicationId { get; set; }
        public Guid JobId { get; set; }
        public string JobTitle { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        
        public Guid ApplicantId { get; set; }
        public string ApplicantName { get; set; } = null!;
        
        public Guid ApplicationStatusId { get; set; }
        public string StatusName { get; set; } = null!;
        public string? StatusColorCode { get; set; }
        
        public string? CoverLetter { get; set; }
        public string? ResumeUrl { get; set; }
        public Guid? AttachmentId { get; set; }
        
        public bool IsShortlisted { get; set; }
        public bool IsRejected { get; set; }
        public string? EmployerNotes { get; set; }
        public DateTime? ReviewedAt { get; set; }
        
        public bool IsWithdrawn { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateApplicationDto
    {
        public Guid JobId { get; set; }
        public string? CoverLetter { get; set; }
        public Guid? ResumeId { get; set; } // Link to uploaded UserResume or new Attachment
        public string? AdditionalInfo { get; set; }
    }

    public class ApplicationStatusHistoryDto
    {
        public Guid HistoryId { get; set; }
        public Guid StatusId { get; set; }
        public string StatusName { get; set; } = null!;
        public string? Notes { get; set; }
        public string? ChangedByRole { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
