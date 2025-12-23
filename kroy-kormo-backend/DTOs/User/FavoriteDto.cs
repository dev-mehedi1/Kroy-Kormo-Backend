using Sob_Need_Server.DTOs.Product;

namespace Sob_Need_Server.DTOs.User
{
    public class FavoriteDto
    {
        public Guid FavoriteId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime CreatedAt { get; set; }
        public ProductDto? Product { get; set; }
    }
}
