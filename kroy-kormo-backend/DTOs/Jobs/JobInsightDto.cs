using Sob_Need_Server.DTOs.Jobs;

namespace Sob_Need_Server.DTOs.Jobs
{
    public class UserInsightDto
    {
        public ProfileCompletenessDto ProfileCompleteness { get; set; } = new();
        public List<SkillAnalysisDto> SkillAnalysis { get; set; } = new();
        public List<JobRecommendationDto> RecommendedJobs { get; set; } = new();
        public List<ActionableStepDto> NextSteps { get; set; } = new();
        public ActivitySummaryDto ActivitySummary { get; set; } = new();
    }

    public class ProfileCompletenessDto
    {
        public int Percentage { get; set; }
        public List<string> MissingFields { get; set; } = new();
    }

    public class SkillAnalysisDto
    {
        public string SkillName { get; set; } = null!;
        public string ProficiencyLevel { get; set; } = null!;
        public int MarketDemandScore { get; set; } // 1-100 based on job postings
        public List<string> RelatedJobs { get; set; } = new();
    }

    public class JobRecommendationDto
    {
        public Guid JobId { get; set; }
        public string Title { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string? CompanyLogo { get; set; }
        public int MatchPercentage { get; set; }
        public List<string> MatchingSkills { get; set; } = new();
        public List<string> MissingSkills { get; set; } = new();
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string? SalaryCurrency { get; set; }
        public bool IsRemote { get; set; }
    }

    public class ActionableStepDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ActionType { get; set; } = null!; // "EditProfile", "AddSkill", "ApplyJob", "CompleteEducation"
        public string ActionUrl { get; set; } = null!;
        public string Priority { get; set; } = null!; // "High", "Medium", "Low"
    }

    public class ActivitySummaryDto
    {
        public int TotalApplications { get; set; }
        public int RecentViews { get; set; }
        public int SavedJobsCount { get; set; }
        public int InterviewsScheduled { get; set; }
    }
}
