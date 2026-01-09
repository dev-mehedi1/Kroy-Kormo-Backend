using System.Collections.Generic;

namespace Sob_Need_Server.DTOs.Jobs
{
    public class JobWithScoreDto : JobListingDto
    {
        public double MatchScore { get; set; }
        public List<string> MatchReasons { get; set; } = new();
    }

    public class TrendingJobDto : JobListingDto
    {
        public double TrendingScore { get; set; }
        public int ViewCount7d { get; set; }
        public int ApplicationCount7d { get; set; }
    }
}
