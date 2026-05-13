using System;

namespace Sob_Need_Server.DTOs.Jobs
{
    public class JobViewDto
    {
        public Guid ViewId { get; set; }
        public Guid JobId { get; set; }
        public Guid? UserId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? DeviceType { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public DateTime ViewedAt { get; set; }
    }

    public class CreateJobViewDto
    {
        public Guid JobId { get; set; }
        public string? ReferrerUrl { get; set; }
    }

    public class JobViewAnalyticsDto
    {
        public int TotalViews { get; set; }
        public int UniqueViews { get; set; }
        public List<ViewByDateDto> ViewsByDate { get; set; } = new();
        public List<ViewByDeviceDto> ViewsByDevice { get; set; } = new();
        public List<ViewByLocationDto> ViewsByLocation { get; set; } = new();
    }

    public class ViewByDateDto
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public class ViewByDeviceDto
    {
        public string DeviceType { get; set; } = null!;
        public int Count { get; set; }
    }

    public class ViewByLocationDto
    {
        public string Country { get; set; } = null!;
        public string? City { get; set; }
        public int Count { get; set; }
    }
}
