using System;
using System.Collections.Generic;

namespace Sob_Need_Server.DTOs.Analytics
{
    public class SellerStatsDto
    {
        public int TotalActiveAds { get; set; }
        public int TotalViews { get; set; }
        public int TotalFavorites { get; set; }
        public int TotalConversations { get; set; }
        public IEnumerable<DailyStatDto> DailyStats { get; set; } = new List<DailyStatDto>();
        public IEnumerable<TopProductStatDto> TopProducts { get; set; } = new List<TopProductStatDto>();
    }

    public class DailyStatDto
    {
        public DateTime Date { get; set; }
        public int Views { get; set; }
        public int Favorites { get; set; }
    }

    public class TopProductStatDto
    {
        public Guid ProductId { get; set; }
        public string Title { get; set; } = null!;
        public int Views { get; set; }
        public int Favorites { get; set; }
        public string? ImageUrl { get; set; }
    }
}
