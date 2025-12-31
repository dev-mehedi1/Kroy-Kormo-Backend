using System;

namespace Sob_Need_Server.DTOs.Product
{
    public class ProductStatusHistoryDto
    {
        public Guid HistoryId { get; set; }
        public Guid StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? Notes { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
