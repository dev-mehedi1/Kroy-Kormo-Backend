using System;
using System.Collections.Generic;

namespace Sob_Need_Server.DTOs.Jobs
{
    public class SavedJobDto
    {
        public Guid SavedJobId { get; set; }
        public Guid UserId { get; set; }
        public Guid JobId { get; set; }
        public string? Notes { get; set; }
        public DateTime SavedAt { get; set; }
        
        // Flattened job details for display
        public string Title { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string? CompanyLogoUrl { get; set; }
        public string JobTypeName { get; set; } = null!;
        public string LocationName { get; set; } = null!;
        public bool IsRemote { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string? SalaryCurrency { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<SkillDto> RequiredSkills { get; set; } = new();
    }

    public class CreateSavedJobDto
    {
        public Guid JobId { get; set; }
        public string? Notes { get; set; }
    }
}
