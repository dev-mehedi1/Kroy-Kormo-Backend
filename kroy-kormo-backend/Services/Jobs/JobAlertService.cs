using AutoMapper;
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
    public class JobAlertService : IJobAlertService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public JobAlertService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<JobAlertDto> CreateAlertAsync(Guid userId, CreateJobAlertDto dto)
        {
            var alert = _mapper.Map<JobAlert>(dto);
            alert.UserId = userId;
            alert.IsActive = true;
            alert.CreatedAt = DateTime.UtcNow;

            _context.JobAlerts.Add(alert);
            await _context.SaveChangesAsync();

            return await GetAlertByIdAsync(alert.AlertId, userId) ?? throw new Exception("Failed to retrieve created alert");
        }

        public async Task<JobAlertDto> UpdateAlertAsync(Guid alertId, Guid userId, UpdateJobAlertDto dto)
        {
            var alert = await _context.JobAlerts.FirstOrDefaultAsync(a => a.AlertId == alertId && a.UserId == userId);
            if (alert == null) throw new KeyNotFoundException("Alert not found");

            _mapper.Map(dto, alert);
            alert.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await GetAlertByIdAsync(alertId, userId) ?? throw new Exception("Failed to retrieve updated alert");
        }

        public async Task<bool> DeleteAlertAsync(Guid alertId, Guid userId)
        {
            var alert = await _context.JobAlerts.FirstOrDefaultAsync(a => a.AlertId == alertId && a.UserId == userId);
            if (alert == null) return false;

            _context.JobAlerts.Remove(alert);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<JobAlertDto>> GetUserAlertsAsync(Guid userId)
        {
            var alerts = await _context.JobAlerts
                .Include(a => a.Category)
                .Include(a => a.JobType)
                .Include(a => a.ExperienceLevel)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<JobAlertDto>>(alerts);
        }

        public async Task<JobAlertDto?> GetAlertByIdAsync(Guid alertId, Guid userId)
        {
            var alert = await _context.JobAlerts
                .Include(a => a.Category)
                .Include(a => a.JobType)
                .Include(a => a.ExperienceLevel)
                .FirstOrDefaultAsync(a => a.AlertId == alertId && a.UserId == userId);

            return _mapper.Map<JobAlertDto>(alert);
        }

        public async Task<bool> ToggleAlertStatusAsync(Guid alertId, Guid userId)
        {
            var alert = await _context.JobAlerts.FirstOrDefaultAsync(a => a.AlertId == alertId && a.UserId == userId);
            if (alert == null) return false;

            alert.IsActive = !alert.IsActive;
            alert.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<JobListingDto>> GetAlertMatchesAsync(Guid alertId, Guid userId)
        {
            var alert = await _context.JobAlerts.FirstOrDefaultAsync(a => a.AlertId == alertId && a.UserId == userId);
            if (alert == null) return Enumerable.Empty<JobListingDto>();

            var query = _context.JobListings
                .Include(j => j.Product)
                .Include(j => j.Employer)
                .Include(j => j.JobType)
                .Include(j => j.ExperienceLevel)
                .Include(j => j.Category)
                .Include(j => j.CompanyProfile)
                .Where(j => j.IsActive && j.JobStatus == "Active")
                .AsQueryable();

            // Apply filters from alert
            if (!string.IsNullOrEmpty(alert.Keywords))
                query = query.Where(j => j.Product!.Title.Contains(alert.Keywords) || j.Product!.Description.Contains(alert.Keywords));

            if (alert.CategoryId.HasValue)
                query = query.Where(j => j.CategoryId == alert.CategoryId.Value);

            if (alert.JobTypeId.HasValue)
                query = query.Where(j => j.JobTypeId == alert.JobTypeId.Value);

            if (alert.ExperienceLevelId.HasValue)
                query = query.Where(j => j.ExperienceLevelId == alert.ExperienceLevelId.Value);

            if (alert.IsRemote.HasValue)
                query = query.Where(j => j.IsRemote == alert.IsRemote.Value);

            if (alert.MinSalary.HasValue)
                query = query.Where(j => j.SalaryMax >= alert.MinSalary.Value);

            if (alert.MaxSalary.HasValue)
                query = query.Where(j => j.SalaryMin <= alert.MaxSalary.Value);

            var items = await query.OrderByDescending(j => j.CreatedAt).Take(50).ToListAsync();
            return _mapper.Map<IEnumerable<JobListingDto>>(items);
        }

        public async Task<int> CheckAlertsForNewJobAsync(JobListingDto job)
        {
            // This would normally be called in a background job or after a job is published
            // Find all active alerts that match this job
            var query = _context.JobAlerts
                .Where(a => a.IsActive)
                .AsQueryable();

            // Matching logic
            var matchingAlerts = await query.ToListAsync();
            var matches = matchingAlerts.Where(a => 
                (string.IsNullOrEmpty(a.Keywords) || job.Title.Contains(a.Keywords) || job.Description.Contains(a.Keywords)) &&
                (!a.CategoryId.HasValue || a.CategoryId == job.CategoryId) &&
                (!a.JobTypeId.HasValue || a.JobTypeId == job.JobTypeId) &&
                (!a.ExperienceLevelId.HasValue || a.ExperienceLevelId == job.ExperienceLevelId) &&
                (!a.IsRemote.HasValue || a.IsRemote == job.IsRemote) &&
                (!a.MinSalary.HasValue || job.SalaryMax >= a.MinSalary) &&
                (!a.MaxSalary.HasValue || job.SalaryMin <= a.MaxSalary)
            ).ToList();

            foreach (var alert in matches)
            {
                alert.MatchCount++;
                alert.LastMatchedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return matches.Count;
        }
    }
}
