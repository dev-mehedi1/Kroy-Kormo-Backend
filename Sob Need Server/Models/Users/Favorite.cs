using Sob_Need_Server.Models.Authentication;
using Sob_Need_Server.Models.Products;
using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Users
{
    public class Favorite
    {
        public Guid FavoriteId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public Guid ProductId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User? User { get; set; }
        public virtual Product? Product { get; set; }
    }
}
