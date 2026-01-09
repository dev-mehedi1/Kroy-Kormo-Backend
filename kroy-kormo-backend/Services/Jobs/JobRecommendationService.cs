using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Jobs;
using Sob_Need_Server.Models.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sob_Need_Server.Services.Jobs
{
    public class JobRecommendationService : IJobRecommendationService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public JobRecommendationService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JobWithScoreDto>> GetRecommendedJobsAsync(Guid userId, int count = 20)
        {
            // 1. Get user skills
            var userSkills = await _context.UserSkills
                .Where(us => us.UserId == userId)
                .Select(us => us.SkillId)
                .ToListAsync();

            if (!userSkills.Any())
            {
                // Fallback to most recent active jobs if user has no skills listed
                var recentJobs = await _context.JobListings
                    .Include(j => j.Product)
                    .Include(j => j.Employer)
                    .Include(j => j.JobType)
                    .Include(j => j.ExperienceLevel)
                    .Include(j => j.Category)
                    .Include(j => j.CompanyProfile)
                    .Include(j => j.RequiredSkills).ThenInclude(rs => rs.Skill)
                    .Where(j => j.IsActive && j.JobStatus == "Active")
                    .OrderByDescending(j => j.CreatedAt)
                    .Take(count)
                    .ToListAsync();

                return recentJobs.Select(j => 
                {
                    var dto = _mapper.Map<JobWithScoreDto>(j);
                    dto.MatchScore = 0;
                    dto.MatchReasons.Add("Showing latest active jobs");
                    return dto;
                });
            }

            // 2. Find jobs that require at least one of these skills
            var candidateJobIds = await _context.JobSkills
                .Where(js => userSkills.Contains(js.SkillId))
                .Select(js => js.JobId)
                .Distinct()
                .ToListAsync();

            var jobs = await _context.JobListings
                .Include(j => j.Product)
                .Include(j => j.Employer)
                .Include(j => j.JobType)
                .Include(j => j.ExperienceLevel)
                .Include(j => j.Category)
                .Include(j => j.CompanyProfile)
                .Include(j => j.RequiredSkills).ThenInclude(rs => rs.Skill)
                .Where(j => j.IsActive && j.JobStatus == "Active" && candidateJobIds.Contains(j.JobId))
                .ToListAsync();

            var scoredJobs = jobs.Select(j => 
            {
                var dto = _mapper.Map<JobWithScoreDto>(j);
                var requiredSkillIds = j.RequiredSkills.Select(rs => rs.SkillId).ToList();
                var matchingSkills = requiredSkillIds.Intersect(userSkills).ToList();
                
                // Scoring
                double skillMatchScore = (double)matchingSkills.Count / (requiredSkillIds.Count > 0 ? requiredSkillIds.Count : 1);
                dto.MatchScore = skillMatchScore * 100;
                
                foreach (var skillId in matchingSkills)
                {
                    var skillName = j.RequiredSkills.First(rs => rs.SkillId == skillId).Skill!.SkillName;
                    dto.MatchReasons.Add($"Matches your skill: {skillName}");
                }

                return dto;
            })
            .OrderByDescending(j => j.MatchScore)
            .OrderByDescending(j => j.CreatedAt)
            .Take(count)
            .ToList();

            return scoredJobs;
        }

        public async Task<IEnumerable<TrendingJobDto>> GetTrendingJobsAsync(int count = 20)
        {
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);

            // Fetch jobs with high views and applications in the last 7 days
            // For now, using the JobListing totals as simplified proxy
            var trendingJobs = await _context.JobListings
                .Include(j => j.Product)
                .Include(j => j.Employer)
                .Include(j => j.JobType)
                .Include(j => j.ExperienceLevel)
                .Include(j => j.Category)
                .Include(j => j.CompanyProfile)
                .Where(j => j.IsActive && j.JobStatus == "Active")
                .OrderByDescending(j => j.ViewCount * 0.6 + j.ApplicationCount * 0.4)
                .Take(count)
                .ToListAsync();

            return trendingJobs.Select(j => 
            {
                var dto = _mapper.Map<TrendingJobDto>(j);
                dto.TrendingScore = j.ViewCount * 0.6 + j.ApplicationCount * 0.4;
                dto.ViewCount7d = j.ViewCount; // Simplified
                dto.ApplicationCount7d = j.ApplicationCount; // Simplified
                return dto;
            });
        }

        public async Task<IEnumerable<JobWithScoreDto>> GetSimilarJobsAsync(Guid jobId, int count = 10)
        {
            var sourceJob = await _context.JobListings
                .Include(j => j.RequiredSkills)
                .FirstOrDefaultAsync(j => j.JobId == jobId);

            if (sourceJob == null) return Enumerable.Empty<JobWithScoreDto>();

            var sourceSkillIds = sourceJob.RequiredSkills.Select(rs => rs.SkillId).ToList();
            
            var similarJobIds = await _context.JobSkills
                .Where(js => js.JobId != jobId && sourceSkillIds.Contains(js.SkillId))
                .GroupBy(js => js.JobId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(count)
                .ToListAsync();

            var jobs = await _context.JobListings
                .Include(j => j.Product)
                .Include(j => j.Employer)
                .Include(j => j.JobType)
                .Include(j => j.ExperienceLevel)
                .Include(j => j.Category)
                .Include(j => j.CompanyProfile)
                .Include(j => j.RequiredSkills).ThenInclude(rs => rs.Skill)
                .Where(j => j.IsActive && j.JobStatus == "Active" && (similarJobIds.Contains(j.JobId) || j.CategoryId == sourceJob.CategoryId))
                .Where(j => j.JobId != jobId)
                .Take(count)
                .ToListAsync();

            return jobs.Select(j => 
            {
                var dto = _mapper.Map<JobWithScoreDto>(j);
                if (j.CategoryId == sourceJob.CategoryId)
                    dto.MatchReasons.Add("Same job category");
                
                var commonSkills = j.RequiredSkills.Select(rs => rs.SkillId).Intersect(sourceSkillIds).Count();
                if (commonSkills > 0)
                    dto.MatchReasons.Add($"{commonSkills} shared skills");

                return dto;
            });
        }

        public async Task<IEnumerable<JobListingDto>> GetNearbyJobsAsync(double lat, double lng, double radiusKm = 50, int count = 20)
        {
            // Simplified: Since we don't have geospatial queries set up in EF Core Npgsql yet for this project,
            // we'll fallback to matching by city/region if provided, or just recent jobs for now.
            // Ideally use PostGIS: .Where(j => j.Location.Distance(userPoint) <= radiusKm * 1000)
            
            var recentJobs = await _context.JobListings
                .Include(j => j.Product)
                .Include(j => j.Employer)
                .Include(j => j.JobType)
                .Include(j => j.ExperienceLevel)
                .Include(j => j.Category)
                .Include(j => j.CompanyProfile)
                .Where(j => j.IsActive && j.JobStatus == "Active")
                .OrderByDescending(j => j.CreatedAt)
                .Take(count)
                .ToListAsync();

            return _mapper.Map<IEnumerable<JobListingDto>>(recentJobs);
        }
    }
}
