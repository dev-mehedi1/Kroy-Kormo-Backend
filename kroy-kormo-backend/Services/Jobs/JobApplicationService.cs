using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models.Products;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Jobs;
using Sob_Need_Server.DTOs.Notification;
using Sob_Need_Server.Interface.Notification;
using Sob_Need_Server.Models.Jobs;
using Sob_Need_Server.SignalR;

namespace Sob_Need_Server.Services.Jobs
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationService _notificationService;

        public JobApplicationService(AppDbContext context, IMapper mapper, IHubContext<NotificationHub> hubContext, INotificationService notificationService)
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
            _notificationService = notificationService;
        }

        public async Task<JobApplicationDto> ApplyForJobAsync(Guid applicantId, CreateApplicationDto dto)
        {
            // Check if already applied
            var existing = await _context.JobApplications
                .AnyAsync(a => a.JobId == dto.JobId && a.ApplicantId == applicantId);
            
            if (existing) throw new InvalidOperationException("You have already applied for this job.");

            var job = await _context.JobListings.FindAsync(dto.JobId);
            if (job == null) throw new KeyNotFoundException("Job listing not found.");

            // Get default status (Applied)
            var status = await _context.ApplicationStatuses
                .FirstOrDefaultAsync(s => s.StatusName == "Applied" || s.Slug == "applied");

            var application = new JobApplication
            {
                JobId = dto.JobId,
                ApplicantId = applicantId,
                ApplicationStatusId = status?.ApplicationStatusId ?? Guid.Empty,
                CoverLetter = dto.CoverLetter,
                AttachmentId = dto.ResumeId,
                CreatedAt = DateTime.UtcNow
            };

            _context.JobApplications.Add(application);
            
            // Record status history
            _context.ApplicationStatusHistories.Add(new ApplicationStatusHistory
            {
                ApplicationId = application.ApplicationId,
                StatusId = application.ApplicationStatusId,
                ChangedByRole = "Applicant",
                CreatedAt = DateTime.UtcNow
            });


            // Update application count on job
            job.ApplicationCount++;

            await _context.SaveChangesAsync();
            return await GetApplicationDetailAsync(application.ApplicationId, applicantId) ?? throw new Exception("Failed to retrieve application");
        }

        public async Task<bool> WithdrawApplicationAsync(Guid applicationId, Guid applicantId)
        {
            var app = await _context.JobApplications.FirstOrDefaultAsync(a => a.ApplicationId == applicationId && a.ApplicantId == applicantId);
            if (app == null) return false;

            app.IsWithdrawn = true;
            app.WithdrawnAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateApplicationStatusAsync(Guid applicationId, Guid employerId, Guid statusId, string? notes)
        {
            var app = await _context.JobApplications
                .Include(a => a.Job).ThenInclude(j => j.Product)
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

            if (app == null || app.Job?.EmployerId != employerId) return false;

            app.ApplicationStatusId = statusId;
            app.ReviewedAt = DateTime.UtcNow;
            app.EmployerNotes = notes;

            _context.ApplicationStatusHistories.Add(new ApplicationStatusHistory
            {
                ApplicationId = applicationId,
                StatusId = statusId,
                Notes = notes,
                ChangedByRole = "Employer",
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            // Notify Applicant
            var newStatus = await _context.ApplicationStatuses.FindAsync(statusId);
            await _notificationService.CreateAsync(new NotificationDto
            {
                 Title = "Application Status Updated",
                 Message = $"Your application for {app.Job?.Product?.Title} has been updated to: {newStatus?.StatusName}. {notes}",
                 Type = "Application",
                 RecipientId = app.ApplicantId,
                 ActionUrl = $"/jobs/apply",
                 IsRead = false
            });

            return true;
        }

        public async Task<PaginatedResponseDto<JobApplicationDto>> GetJobApplicationsAsync(Guid jobId, Guid employerId, ApplicationFilterDto filter)
        {
            var query = _context.JobApplications
                .Include(a => a.Applicant)
                .Include(a => a.Status)
                .Include(a => a.Job)
                .Where(a => a.JobId == jobId && a.Job!.EmployerId == employerId && !a.IsWithdrawn)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(a => a.Applicant!.FullName.Contains(filter.Keyword) || a.CoverLetter!.Contains(filter.Keyword));
            }

            if (!string.IsNullOrEmpty(filter.Status) && filter.Status != "All")
            {
                query = query.Where(a => a.Status.StatusName == filter.Status);
            }

            query = filter.SortBy?.ToLower() switch
            {
                "oldest" => query.OrderBy(a => a.CreatedAt),
                "name" => query.OrderBy(a => a.Applicant!.FullName),
                _ => query.OrderByDescending(a => a.CreatedAt)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<JobApplicationDto>>(items);

            return new PaginatedResponseDto<JobApplicationDto>
            {
                Data = dtos,
                TotalCount = totalCount,
                Page = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize),
                HasNextPage = filter.PageNumber < (int)Math.Ceiling(totalCount / (double)filter.PageSize),
                HasPreviousPage = filter.PageNumber > 1
            };
        }

        public async Task<JobApplicationDto?> GetApplicationDetailAsync(Guid applicationId, Guid userId)
        {
            var app = await _context.JobApplications
                .Include(a => a.Applicant)
                .Include(a => a.Status)
                .Include(a => a.Job).ThenInclude(j => j!.Product)
                .Include(a => a.Job).ThenInclude(j => j!.CompanyProfile)
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId && (a.ApplicantId == userId || a.Job!.EmployerId == userId));

            return _mapper.Map<JobApplicationDto>(app);
        }

        public async Task<PaginatedResponseDto<JobApplicationDto>> GetUserApplicationsAsync(Guid applicantId, ApplicationFilterDto filter)
        {
            var query = _context.JobApplications
                .Include(a => a.Job).ThenInclude(j => j!.Product)
                .Include(a => a.Job).ThenInclude(j => j!.CompanyProfile)
                .Include(a => a.Status)
                .Where(a => a.ApplicantId == applicantId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(a => a.Job!.Product!.Title.Contains(filter.Keyword) || a.Job!.CompanyProfile!.CompanyName.Contains(filter.Keyword));
            }

            if (!string.IsNullOrEmpty(filter.Status) && filter.Status != "All")
            {
                // Simple status filtering for candidate dashboard tabs
                if (filter.Status == "Active")
                {
                    query = query.Where(a => new[] { "Pending", "Reviewing", "Screening" }.Contains(a.Status.StatusName));
                }
                else if (filter.Status == "Interviewing")
                {
                    query = query.Where(a => new[] { "Interview", "Interviewing" }.Contains(a.Status.StatusName));
                }
                else if (filter.Status == "Archived")
                {
                    query = query.Where(a => new[] { "Rejected", "Withdrawn", "Offered" }.Contains(a.Status.StatusName));
                }
                else
                {
                    query = query.Where(a => a.Status.StatusName == filter.Status);
                }
            }

            query = filter.SortBy?.ToLower() switch
            {
                "oldest" => query.OrderBy(a => a.CreatedAt),
                _ => query.OrderByDescending(a => a.CreatedAt)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<JobApplicationDto>>(items);

            return new PaginatedResponseDto<JobApplicationDto>
            {
                Data = dtos,
                TotalCount = totalCount,
                Page = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize),
                HasNextPage = filter.PageNumber < (int)Math.Ceiling(totalCount / (double)filter.PageSize),
                HasPreviousPage = filter.PageNumber > 1
            };
        }

        public async Task<IEnumerable<ApplicationStatusHistoryDto>> GetApplicationHistoryAsync(Guid applicationId)
        {
            var history = await _context.ApplicationStatusHistories
                .Include(h => h.Status)
                .Where(h => h.ApplicationId == applicationId)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ApplicationStatusHistoryDto>>(history);
        }
    }
}
