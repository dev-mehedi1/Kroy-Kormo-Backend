using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Jobs;
using Sob_Need_Server.Models.Authentication;
using Sob_Need_Server.Services.Jobs;
using System.Linq;

namespace Services.Jobs
{
    public class JobInsightService(AppDbContext context) : IJobInsightService
    {
        private readonly AppDbContext _context = context;

        public async Task<UserInsightDto> GetUserInsightsAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Skills).ThenInclude(us => us.Skill)
                .Include(u => u.JobApplications)
                .Include(u => u.Experiences)
                .Include(u => u.Educations)
                .Include(u => u.SavedJobs)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null) return new UserInsightDto();

            var insights = new UserInsightDto();

            // 1. Profile Completeness
            insights.ProfileCompleteness = CalculateProfileCompleteness(user);

            // 2. Activity Summary
            insights.ActivitySummary = new ActivitySummaryDto
            {
                TotalApplications = user.JobApplications.Count,
                RecentViews = await _context.JobViews.CountAsync(v => v.UserId == userId && v.ViewedAt > DateTime.UtcNow.AddDays(-30)),
                SavedJobsCount = user.SavedJobs.Count,
                InterviewsScheduled = await _context.Interviews.CountAsync(i => i.CandidateId == userId && i.Status == "Scheduled")
            };

            // 3. User Skills for Matching
            var userSkillIds = user.Skills.Select(s => s.SkillId).ToList();

            // 4. Job Recommendations (Advanced Matching)
            insights.RecommendedJobs = await GetJobRecommendations(userId, userSkillIds);

            // 5. Skill Analysis (Demand based on active jobs)
            insights.SkillAnalysis = await AnalyzeSkills(user);

            // 6. Actionable Next Steps
            insights.NextSteps = GenerateNextSteps(user, insights);

            return insights;
        }

        private ProfileCompletenessDto CalculateProfileCompleteness(User user)
        {
            var missingFields = new List<string>();
            int score = 0;
            int totalWeights = 100;

            // Simple heuristic weights
            if (!string.IsNullOrEmpty(user.FullName)) score += 10; else missingFields.Add("Full Name");
            if (!string.IsNullOrEmpty(user.Bio)) score += 5; else missingFields.Add("Bio");
            if (!string.IsNullOrEmpty(user.AboutMe)) score += 10; else missingFields.Add("Professional Summary");
            if (!string.IsNullOrEmpty(user.ProfilePicture)) score += 5; else missingFields.Add("Profile Picture");
            if (user.Skills.Any()) score += 20; else missingFields.Add("Skills");
            if (user.Experiences.Any()) score += 20; else missingFields.Add("Work Experience");
            if (user.Educations.Any()) score += 15; else missingFields.Add("Education");
            if (!user.IsFresher && user.TotalExperienceMonths > 0) score += 5; 
            if (user.Resumes.Any()) score += 10; else missingFields.Add("Resume/CV");

            return new ProfileCompletenessDto
            {
                Percentage = Math.Min(score, 100),
                MissingFields = missingFields
            };
        }

        private async Task<List<JobRecommendationDto>> GetJobRecommendations(Guid userId, List<Guid> userSkillIds)
        {
            // Fetch active jobs with their required skills
            var activeJobs = await _context.JobListings
                .Include(j => j.Product)
                .Include(j => j.CompanyProfile)
                .Include(j => j.RequiredSkills).ThenInclude(rs => rs.Skill)
                .Where(j => j.IsActive && j.JobStatus == "Active" && j.EmployerId != userId)
                .OrderByDescending(j => j.CreatedAt)
                .Take(50) // Limit search space
                .ToListAsync();

            var recommendations = new List<JobRecommendationDto>();

            foreach (var job in activeJobs)
            {
                var jobSkillIds = job.RequiredSkills.Select(s => s.SkillId).ToList();
                var matchingSkills = job.RequiredSkills.Where(s => userSkillIds.Contains(s.SkillId)).ToList();
                var missingSkills = job.RequiredSkills.Where(s => !userSkillIds.Contains(s.SkillId)).ToList();

                int matchPercentage = 0;
                if (jobSkillIds.Count > 0)
                {
                    matchPercentage = (int)((double)matchingSkills.Count / jobSkillIds.Count * 100);
                }
                else
                {
                    matchPercentage = 50; // Neutral baseline if no skills specified
                }

                // Boost match if titles are similar (very basic)
                // In a real app, use semantic search

                recommendations.Add(new JobRecommendationDto
                {
                    JobId = job.JobId,
                    Title = job.Product?.Title ?? "Untitled Job",
                    CompanyName = job.CompanyProfile?.CompanyName ?? "Unknown Company",
                    CompanyLogo = job.CompanyProfile?.LogoUrl,
                    MatchPercentage = matchPercentage,
                    MatchingSkills = matchingSkills.Select(s => s.Skill?.SkillName ?? "Unknown").ToList(),
                    MissingSkills = missingSkills.Select(s => s.Skill?.SkillName ?? "Unknown").ToList(),
                    SalaryMin = job.SalaryMin,
                    SalaryMax = job.SalaryMax,
                    SalaryCurrency = job.SalaryCurrency,
                    IsRemote = job.IsRemote
                });
            }

            return recommendations
                .OrderByDescending(r => r.MatchPercentage)
                .ThenByDescending(r => r.MatchPercentage > 0)
                .Take(6)
                .ToList();
        }

        private async Task<List<SkillAnalysisDto>> AnalyzeSkills(User user)
        {
            var analysis = new List<SkillAnalysisDto>();
            
            // Get frequency of skills in active job postings to estimate "Market Demand"
            var skillFrequencies = await _context.JobSkills
                .GroupBy(js => js.SkillId)
                .Select(g => new { SkillId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.SkillId, x => x.Count);

            int maxCount = skillFrequencies.Values.DefaultIfEmpty(1).Max();

            foreach (var userSkill in user.Skills)
            {
                int frequency = skillFrequencies.GetValueOrDefault(userSkill.SkillId, 0);
                int score = (int)((double)frequency / maxCount * 100);

                analysis.Add(new SkillAnalysisDto
                {
                    SkillName = userSkill.Skill?.SkillName ?? "Unknown",
                    ProficiencyLevel = userSkill.ProficiencyLevel ?? "Not Specified",
                    MarketDemandScore = score,
                    RelatedJobs = await _context.JobListings
                        .Where(j => j.RequiredSkills.Any(rs => rs.SkillId == userSkill.SkillId))
                        .Select(j => j.Product!.Title)
                        .Take(3)
                        .ToListAsync()
                });
            }

            return analysis.OrderByDescending(a => a.MarketDemandScore).ToList();
        }

        private List<ActionableStepDto> GenerateNextSteps(User user, UserInsightDto insights)
        {
            var steps = new List<ActionableStepDto>();

            if (insights.ProfileCompleteness.Percentage < 100)
            {
                if (insights.ProfileCompleteness.MissingFields.Contains("Resume/CV"))
                {
                    steps.Add(new ActionableStepDto
                    {
                        Title = "Upload Your Resume",
                        Description = "Users with resumes are 5x more likely to get hired.",
                        ActionType = "EditProfile",
                        ActionUrl = "/dashboard/profile",
                        Priority = "High"
                    });
                }

                if (insights.ProfileCompleteness.MissingFields.Contains("Skills"))
                {
                    steps.Add(new ActionableStepDto
                    {
                        Title = "Add Your Skills",
                        Description = "Add at least 5 skills to improve matching accuracy.",
                        ActionType = "AddSkill",
                        ActionUrl = "/dashboard/profile",
                        Priority = "High"
                    });
                }
            }

            // Suggest applying if highly matched jobs found
            var highMatch = insights.RecommendedJobs.FirstOrDefault(r => r.MatchPercentage >= 80);
            if (highMatch != null)
            {
                steps.Add(new ActionableStepDto
                {
                    Title = "Apply for Top Match",
                    Description = $"You're an 80%+ match for {highMatch.Title} at {highMatch.CompanyName}.",
                    ActionType = "ApplyJob",
                    ActionUrl = $"/jobs/job/{highMatch.JobId}",
                    Priority = "Medium"
                });
            }

            // Suggest adding specific missing skills from recommendations
            var topJob = insights.RecommendedJobs.FirstOrDefault();
            if (topJob != null && topJob.MissingSkills.Any())
            {
                var missing = topJob.MissingSkills.First();
                steps.Add(new ActionableStepDto
                {
                    Title = $"Master {missing}",
                    Description = $"This skill is required for {topJob.Title} and many other top roles.",
                    ActionType = "AddSkill",
                    ActionUrl = "/dashboard/profile",
                    Priority = "Low"
                });
            }

            return steps.OrderBy(s => s.Priority == "High" ? 0 : s.Priority == "Medium" ? 1 : 2).ToList();
        }
    }
}
