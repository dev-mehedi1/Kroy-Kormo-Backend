namespace Sob_Need_Server.DTOs.Product
{
    public class ProductStatisticsDto
    {
        public Guid ProductId { get; set; }
        public int ViewCount { get; set; }
        public int FavoriteCount { get; set; }
        public int TotalViews { get; set; }
        public int UniqueViews { get; set; }
        public DateTime? LastViewedAt { get; set; }
    }
}
