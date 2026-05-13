using Sob_Need_Server.DTOs.Jobs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sob_Need_Server.Services.Jobs
{
    public interface IJobRecommendationService
    {
        Task<IEnumerable<JobWithScoreDto>> GetRecommendedJobsAsync(Guid userId, int count = 20);
        Task<IEnumerable<TrendingJobDto>> GetTrendingJobsAsync(int count = 20);
        Task<IEnumerable<JobWithScoreDto>> GetSimilarJobsAsync(Guid jobId, int count = 10);
        Task<IEnumerable<JobListingDto>> GetNearbyJobsAsync(double lat, double lng, double radiusKm = 50, int count = 20);
    }
}
