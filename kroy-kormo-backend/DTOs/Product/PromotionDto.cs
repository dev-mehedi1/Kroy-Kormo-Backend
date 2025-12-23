using System;

namespace Sob_Need_Server.DTOs.Product
{
    public class PromotionDto
    {
        public Guid PromotionId { get; set; }
        public Guid ProductId { get; set; }
        public string PromotionType { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
