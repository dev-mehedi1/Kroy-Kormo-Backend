namespace Sob_Need_Server.DTOs.User
{
    public class ReviewDto
    {
        public Guid ReviewId { get; set; }
        public Guid SellerId { get; set; }
        public string? SellerName { get; set; }
        public Guid ReviewerId { get; set; }
        public string? ReviewerName { get; set; }
        public Guid? ProductId { get; set; }
        public string? ProductTitle { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
