using Sob_Need_Server.Models.Authentication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sob_Need_Server.Models.Jobs
{
    public class JobOffer
    {
        [Key]
        public Guid OfferId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid ApplicationId { get; set; }
        
        [Required]
        public Guid EmployerId { get; set; }
        
        [Required]
        public Guid CandidateId { get; set; }
        
        // Compensation
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalaryOffered { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string SalaryCurrency { get; set; } = "USD";
        
        [Required]
        [MaxLength(20)]
        public string SalaryPeriod { get; set; } = "Yearly"; // Hourly, Monthly, Yearly
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BonusAmount { get; set; }
        
        [MaxLength(200)]
        public string? BonusDescription { get; set; }
        
        // Job Details
        [Required]
        [MaxLength(200)]
        public string JobTitle { get; set; } = null!;
        
        [MaxLength(100)]
        public string? Department { get; set; }
        
        public DateTime? StartDate { get; set; }
        
        [MaxLength(50)]
        public string? EmploymentType { get; set; } // Full-time, Part-time, Contract
        
        // Benefits and Terms
        [MaxLength(3000)]
        public string? Benefits { get; set; }
        
        [MaxLength(5000)]
        public string? Terms { get; set; } // Employment terms and conditions
        
        public int? ProbationPeriodMonths { get; set; }
        
        public int? NoticePeriodDays { get; set; }
        
        [MaxLength(500)]
        public string? WorkLocation { get; set; }
        
        [Required]
        public bool IsRemote { get; set; } = false;
        
        // Offer Status
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected, Negotiating, Withdrawn, Expired
        
        public DateTime? ExpiresAt { get; set; }
        
        // Candidate Response
        [MaxLength(2000)]
        public string? CandidateResponse { get; set; }
        
        public DateTime? RespondedAt { get; set; }
        
        // Negotiation
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CounterOfferSalary { get; set; }
        
        [MaxLength(1000)]
        public string? CounterOfferNotes { get; set; }
        
        // Acceptance
        public DateTime? AcceptedAt { get; set; }
        
        public DateTime? RejectedAt { get; set; }
        
        [MaxLength(500)]
        public string? RejectionReason { get; set; }
        
        // Contract
        public Guid? ContractAttachmentId { get; set; }
        
        public bool ContractSigned { get; set; } = false;
        
        public DateTime? ContractSignedAt { get; set; }
        
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
