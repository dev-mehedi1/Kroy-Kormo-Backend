using Models.Products;
using Sob_Need_Server.Models.Products;
using Sob_Need_Server.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Authentication
{
    public class User
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = null!;
        
        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = null!;
        
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;
        
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = null!;
        
        [MaxLength(20)]
        public string? Phone { get; set; }
        
        [MaxLength(500)]
        public string? ProfilePicture { get; set; }
        
        [MaxLength(1000)]
        public string? Bio { get; set; }
        
        [MaxLength(200)]
        public string? Address { get; set; }
        
        public Guid? UserTypeId { get; set; }
        
        [Required]
        public bool IsVerified { get; set; } = false;
        
        [Required]
        public bool IsActive { get; set; } = true;
        
        public DateTime? LastLoginAt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual UserType? UserType { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public virtual ICollection<SavedSearch> SavedSearches { get; set; } = new List<SavedSearch>();
        public virtual ICollection<Review> ReviewsGiven { get; set; } = new List<Review>();
        public virtual ICollection<Review> ReviewsReceived { get; set; } = new List<Review>();
        public virtual ICollection<ProductView> ProductViews { get; set; } = new List<ProductView>();
        
        // Job-related navigation properties
        public virtual ICollection<Models.Jobs.JobListing> JobListings { get; set; } = new List<Models.Jobs.JobListing>();
        public virtual ICollection<Models.Jobs.JobApplication> JobApplications { get; set; } = new List<Models.Jobs.JobApplication>();
        public virtual ICollection<UserEducation> Educations { get; set; } = new List<UserEducation>();
        public virtual ICollection<UserExperience> Experiences { get; set; } = new List<UserExperience>();
        public virtual ICollection<UserSkill> Skills { get; set; } = new List<UserSkill>();
        public virtual ICollection<UserResume> Resumes { get; set; } = new List<UserResume>();
        public virtual ICollection<Models.Jobs.SavedJob> SavedJobs { get; set; } = new List<Models.Jobs.SavedJob>();
        public virtual ICollection<Models.Jobs.JobAlert> JobAlerts { get; set; } = new List<Models.Jobs.JobAlert>();
        public virtual Models.Jobs.CompanyProfile? CompanyProfile { get; set; }
    }
}
