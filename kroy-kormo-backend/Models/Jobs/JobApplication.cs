using Models.Attachment;
using Sob_Need_Server.Models.Authentication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sob_Need_Server.Models.Jobs
{
    public class JobApplication
    {
        [Key]
        public Guid ApplicationId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid JobId { get; set; }
        
        [Required]
        public Guid ApplicantId { get; set; } // UserId
        
        [Required]
        public Guid ApplicationStatusId { get; set; }
        
        // Application Content
        [MaxLength(5000)]
        public string? CoverLetter { get; set; }
        
        [MaxLength(500)]
        public string? ResumeUrl { get; set; }
        
        public Guid? AttachmentId { get; set; } // Resume file attachment
        
        [MaxLength(2000)]
        public string? AdditionalInfo { get; set; } // Any additional information from applicant
        
        // Employer Actions
        [Required]
        public bool IsShortlisted { get; set; } = false;
        
        [Required]
        public bool IsRejected { get; set; } = false;
        
        [MaxLength(2000)]
        public string? EmployerNotes { get; set; }
        
        public DateTime? ReviewedAt { get; set; }
        
        public Guid? ReviewedBy { get; set; } // Employer who reviewed
        
        // Rating (1-5 stars, optional employer rating of candidate)
        public int? CandidateRating { get; set; }
        
        // Privacy Settings
        [Column(TypeName = "text")]
        public string? HiddenFromEmployers { get; set; } // JSON array of company IDs to hide from
        
        // Application Source
        [MaxLength(100)]
        public string? ApplicationSource { get; set; } // Direct, JobAlert, Recommendation, etc.
        
        // Withdrawal
        [Required]
        public bool IsWithdrawn { get; set; } = false;
        
        public DateTime? WithdrawnAt { get; set; }
        
        [MaxLength(500)]
        public string? WithdrawalReason { get; set; }
        
        // Metadata
        [Required]
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual JobListing? Job { get; set; }
        public virtual User? Applicant { get; set; }
        public virtual ApplicationStatus? Status { get; set; }
        public virtual Attachment? Resume { get; set; }
        public virtual ICollection<ApplicationStatusHistory> StatusHistory { get; set; } = new List<ApplicationStatusHistory>();
        public virtual ICollection<Interview> Interviews { get; set; } = new List<Interview>();
        public virtual ICollection<JobOffer> Offers { get; set; } = new List<JobOffer>();
    }
}
