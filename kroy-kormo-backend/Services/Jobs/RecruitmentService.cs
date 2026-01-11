using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Jobs;
using Sob_Need_Server.Models.Jobs;

namespace Sob_Need_Server.Services.Jobs
{
    public class RecruitmentService : IRecruitmentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RecruitmentService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CompanyProfileDto> CreateCompanyProfileAsync(Guid userId, CompanyProfileDto dto)
        {
            var profile = _mapper.Map<CompanyProfile>(dto);
            profile.UserId = userId;
            profile.Slug = dto.CompanyName.ToLower().Replace(" ", "-") + "-" + Guid.NewGuid().ToString().Substring(0, 5);
            
            _context.CompanyProfiles.Add(profile);
            await _context.SaveChangesAsync();
            return _mapper.Map<CompanyProfileDto>(profile);
        }

        public async Task<CompanyProfileDto> UpdateCompanyProfileAsync(Guid userId, CompanyProfileDto dto)
        {
            var profile = await _context.CompanyProfiles.FirstOrDefaultAsync(p => p.CompanyId == dto.CompanyId && p.UserId == userId);
            if (profile == null) throw new KeyNotFoundException("Company profile not found or unauthorized.");

            _mapper.Map(dto, profile);
            profile.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return _mapper.Map<CompanyProfileDto>(profile);
        }

        public async Task<CompanyProfileDto?> GetCompanyProfileAsync(Guid companyId)
        {
            var profile = await _context.CompanyProfiles.FindAsync(companyId);
            return _mapper.Map<CompanyProfileDto>(profile);
        }

        public async Task<IEnumerable<CompanyProfileDto>> GetUserCompanyProfilesAsync(Guid userId)
        {
            var profiles = await _context.CompanyProfiles.Where(p => p.UserId == userId).ToListAsync();
            return _mapper.Map<IEnumerable<CompanyProfileDto>>(profiles);
        }

        public async Task<InterviewDto> ScheduleInterviewAsync(Guid employerId, InterviewDto dto)
        {
            var app = await _context.JobApplications
                .Include(a => a.Job)
                .FirstOrDefaultAsync(a => a.ApplicationId == dto.ApplicationId);

            if (app == null || app.Job?.EmployerId != employerId) 
                throw new UnauthorizedAccessException("Unauthorized or application not found.");

            var interview = _mapper.Map<Interview>(dto);
            interview.EmployerId = employerId;
            interview.CandidateId = app.ApplicantId;
            interview.Status = "Scheduled";
            interview.CreatedAt = DateTime.UtcNow;

            _context.Interviews.Add(interview);
            
            // Auto update application status to 'Interviewing' or similar if needed
            
            await _context.SaveChangesAsync();
            return _mapper.Map<InterviewDto>(interview);
        }

        public async Task<bool> UpdateInterviewStatusAsync(Guid interviewId, Guid userId, string status)
        {
            var interview = await _context.Interviews.FindAsync(interviewId);
            if (interview == null || (interview.EmployerId != userId && interview.CandidateId != userId)) 
                return false;

            interview.Status = status;
            interview.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<InterviewDto>> GetJobInterviewsAsync(Guid jobId, Guid employerId)
        {
            var interviews = await _context.Interviews
                .Include(i => i.Application)
                .Include(i => i.Candidate)
                .Where(i => i.Application!.JobId == jobId && i.EmployerId == employerId)
                .OrderByDescending(i => i.ScheduledAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InterviewDto>>(interviews);
        }

        public async Task<IEnumerable<InterviewDto>> GetMyInterviewsAsync(Guid employerId)
        {
            var interviews = await _context.Interviews
                .Include(i => i.Application)
                .Include(i => i.Candidate)
                .Where(i => i.EmployerId == employerId)
                .OrderByDescending(i => i.ScheduledAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InterviewDto>>(interviews);
        }

        public async Task<JobOfferDto> CreateOfferAsync(Guid employerId, JobOfferDto dto)
        {
            var app = await _context.JobApplications
                .Include(a => a.Job)
                .FirstOrDefaultAsync(a => a.ApplicationId == dto.ApplicationId);

            if (app == null || app.Job?.EmployerId != employerId) 
                throw new UnauthorizedAccessException();

            var offer = _mapper.Map<JobOffer>(dto);
            offer.EmployerId = employerId;
            offer.CandidateId = app.ApplicantId;
            offer.Status = "Pending";
            offer.CreatedAt = DateTime.UtcNow;

            _context.JobOffers.Add(offer);
            await _context.SaveChangesAsync();
            return _mapper.Map<JobOfferDto>(offer);
        }

        public async Task<bool> RespondToOfferAsync(Guid offerId, Guid applicantId, string response, string? notes)
        {
            var offer = await _context.JobOffers.FindAsync(offerId);
            if (offer == null || offer.CandidateId != applicantId) return false;

            offer.Status = response; // Accepted, Rejected, CounterOffer
            offer.RespondedAt = DateTime.UtcNow;
            offer.CandidateResponse = notes;

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<JobStatisticsDto> GetJobStatisticsAsync(Guid jobId, Guid employerId)
        {
            var job = await _context.JobListings
                .Include(j => j.Applications).ThenInclude(a => a.Status)
                .FirstOrDefaultAsync(j => j.JobId == jobId && j.EmployerId == employerId);

            if (job == null) return new JobStatisticsDto();

            var stats = new JobStatisticsDto
            {
                ViewCount = job.ViewCount,
                ApplicationCount = job.Applications.Count,
                ShortlistedCount = job.Applications.Count(a => a.Status?.Slug == "shortlisted"),
                InterviewCount = await _context.Interviews.CountAsync(i => i.Application!.JobId == jobId),
                ApplicationsByStatus = job.Applications
                    .GroupBy(a => a.Status?.StatusName ?? "Unknown")
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return stats;
        }
    }
}
