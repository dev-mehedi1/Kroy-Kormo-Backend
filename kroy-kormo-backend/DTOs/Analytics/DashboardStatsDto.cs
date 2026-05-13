using System;
using System.Collections.Generic;

namespace Sob_Need_Server.DTOs.Analytics
{
    public class DashboardStatsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalListings { get; set; }
        public int ActiveListings { get; set; }
        public decimal TotalRevenue { get; set; } // For future use
        
        public List<DailyStatDto> UserGrowth { get; set; } = new();
        public List<CategoryStatDto> CategoryDistribution { get; set; } = new();
    }

    public class CategoryStatDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
