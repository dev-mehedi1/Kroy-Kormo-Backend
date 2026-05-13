using System;

namespace Sob_Need_Server.DTOs.Jobs
{
    public class ApplicationFilterDto
    {
        public string? Keyword { get; set; }
        public string? Status { get; set; }
        public Guid? JobId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; } // "newest", "oldest", "name"
    }
}
