using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Jobs;
using Sob_Need_Server.Models.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sob_Need_Server.Services.Jobs
{
    public class JobViewService : IJobViewService
    {
        private readonly AppDbContext _context;

        public JobViewService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> TrackViewAsync(Guid? userId, string ipAddress, string userAgent, CreateJobViewDto dto)
        {
            // Check for duplicate view within last 1 hour
            var lastView = await _context.JobViews
                .Where(v => v.JobId == dto.JobId && v.IpAddress == ipAddress && v.ViewedAt > DateTime.UtcNow.AddHours(-1))
                .OrderByDescending(v => v.ViewedAt)
                .FirstOrDefaultAsync();

            if (lastView != null) return false;

            var jobView = new JobView
            {
                JobId = dto.JobId,
                UserId = userId,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                ReferrerUrl = dto.ReferrerUrl,
                ViewedAt = DateTime.UtcNow,
                // These could be extracted from UserAgent or a GeoIP service if available
                DeviceType = ParseDeviceType(userAgent),
                // Location info would ideally come from a GeoIP service
            };

            _context.JobViews.Add(jobView);
            
            // Increment view count on job listing
            var job = await _context.JobListings.FindAsync(dto.JobId);
            if (job != null)
            {
                job.ViewCount++;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<JobViewAnalyticsDto> GetJobViewsAnalyticsAsync(Guid jobId, int days = 30)
        {
            var startDate = DateTime.UtcNow.AddDays(-days);
            
            var views = await _context.JobViews
                .Where(v => v.JobId == jobId && v.ViewedAt >= startDate)
                .ToListAsync();

            var analytics = new JobViewAnalyticsDto
            {
                TotalViews = views.Count,
                UniqueViews = views.Select(v => v.IpAddress).Distinct().Count(),
                ViewsByDate = views
                    .GroupBy(v => v.ViewedAt.Date)
                    .Select(g => new ViewByDateDto { Date = g.Key, Count = g.Count() })
                    .OrderBy(v => v.Date)
                    .ToList(),
                ViewsByDevice = views
                    .GroupBy(v => v.DeviceType ?? "Unknown")
                    .Select(g => new ViewByDeviceDto { DeviceType = g.Key, Count = g.Count() })
                    .ToList(),
                ViewsByLocation = views
                    .GroupBy(v => new { v.Country, v.City })
                    .Select(g => new ViewByLocationDto { Country = g.Key.Country ?? "Unknown", City = g.Key.City, Count = g.Count() })
                    .ToList()
            };

            return analytics;
        }

        public async Task<IEnumerable<JobViewDto>> GetUserViewHistoryAsync(Guid userId, int count = 20)
        {
            var views = await _context.JobViews
                .Where(v => v.UserId == userId)
                .OrderByDescending(v => v.ViewedAt)
                .Take(count)
                .Select(v => new JobViewDto
                {
                    ViewId = v.ViewId,
                    JobId = v.JobId,
                    UserId = v.UserId,
                    IpAddress = v.IpAddress,
                    UserAgent = v.UserAgent,
                    DeviceType = v.DeviceType,
                    Country = v.Country,
                    City = v.City,
                    ViewedAt = v.ViewedAt
                })
                .ToListAsync();

            return views;
        }

        private string ParseDeviceType(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent)) return "Unknown";
            userAgent = userAgent.ToLower();

            if (userAgent.Contains("mobile") || userAgent.Contains("android") || userAgent.Contains("iphone"))
                return "Mobile";
            if (userAgent.Contains("tablet") || userAgent.Contains("ipad"))
                return "Tablet";
            
            return "Desktop";
        }
    }
}
