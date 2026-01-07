using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Jobs;
using Sob_Need_Server.Models.Jobs;

namespace Sob_Need_Server.Services.Jobs
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public JobApplicationService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                .Include(a => a.Job)
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
            return true;
        }

        public async Task<IEnumerable<JobApplicationDto>> GetJobApplicationsAsync(Guid jobId, Guid employerId)
        {
            var apps = await _context.JobApplications
                .Include(a => a.Applicant)
                .Include(a => a.Status)
                .Include(a => a.Job)
                .Where(a => a.JobId == jobId && a.Job!.EmployerId == employerId && !a.IsWithdrawn)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<JobApplicationDto>>(apps);
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

        public async Task<IEnumerable<JobApplicationDto>> GetUserApplicationsAsync(Guid applicantId)
        {
            var apps = await _context.JobApplications
                .Include(a => a.Job).ThenInclude(j => j!.Product)
                .Include(a => a.Job).ThenInclude(j => j!.CompanyProfile)
                .Include(a => a.Status)
                .Where(a => a.ApplicantId == applicantId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<JobApplicationDto>>(apps);
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
