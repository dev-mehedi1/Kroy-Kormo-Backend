using Sob_Need_Server.Models.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Jobs
{
    public class Interview
    {
        [Key]
        public Guid InterviewId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid ApplicationId { get; set; }
        
        [Required]
        public Guid EmployerId { get; set; }
        
        [Required]
        public Guid CandidateId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string InterviewType { get; set; } = null!; // Phone, Video, In-Person, Panel
        
        [Required]
        public DateTime ScheduledAt { get; set; }
        
        public int DurationMinutes { get; set; } = 60;
        
        [MaxLength(500)]
        public string? Location { get; set; } // Physical address or "Remote"
        
        [MaxLength(500)]
        public string? MeetingLink { get; set; } // Zoom, Teams, Google Meet link
        
        [MaxLength(2000)]
        public string? Notes { get; set; } // Instructions for candidate
        
        [MaxLength(100)]
        public string? InterviewerName { get; set; }
        
        [MaxLength(100)]
        public string? InterviewerEmail { get; set; }
        
        [MaxLength(20)]
        public string? InterviewerPhone { get; set; }
        
        // Interview Status
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Scheduled"; // Scheduled, Confirmed, Completed, Cancelled, Rescheduled, NoShow
        
        [MaxLength(3000)]
        public string? Feedback { get; set; } // Employer feedback after interview
        
        public int? Rating { get; set; } // 1-5 rating of candidate performance
        
        // Post-Interview Outcome
        [MaxLength(50)]
        public string? Outcome { get; set; } // "Pending", "Accepted", "Rejected"
        
        public DateTime? CompletedAt { get; set; }
        
        [MaxLength(1000)]
        public string? RejectionReason { get; set; } // Optional professional reason for rejection
        
        // Candidate Response
        public bool? CandidateConfirmed { get; set; }
        
        public DateTime? ConfirmedAt { get; set; }
        
        [MaxLength(500)]
        public string? CandidateNotes { get; set; }
        
        // Cancellation
        public Guid? CancelledBy { get; set; }
        
        public DateTime? CancelledAt { get; set; }
        
        [MaxLength(500)]
        public string? CancellationReason { get; set; }
        
        // Rescheduling
        public Guid? RescheduledFrom { get; set; } // Original interview ID
        
        public DateTime? RescheduledAt { get; set; }
        
        // Metadata
        [Required]
        public bool IsActive { get; set; } = true;
        
        
        [MaxLength(100)]
        public string? CreatedBy { get; set; }
        
        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual JobApplication? Application { get; set; }
        public virtual User? Employer { get; set; }
        public virtual User? Candidate { get; set; }
    }
}
