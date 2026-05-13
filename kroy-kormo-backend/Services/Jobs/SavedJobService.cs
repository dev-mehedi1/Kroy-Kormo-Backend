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
    public class SavedJobService : ISavedJobService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SavedJobService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SavedJobDto> SaveJobAsync(Guid userId, CreateSavedJobDto dto)
        {
            var alreadySaved = await _context.SavedJobs
                .FirstOrDefaultAsync(s => s.UserId == userId && s.JobId == dto.JobId);

            if (alreadySaved != null)
                return _mapper.Map<SavedJobDto>(alreadySaved);

            var savedJob = new SavedJob
            {
                UserId = userId,
                JobId = dto.JobId,
                Notes = dto.Notes,
                SavedAt = DateTime.UtcNow
            };

            _context.SavedJobs.Add(savedJob);
            await _context.SaveChangesAsync();

            // Reload to get navigation properties for the DTO
            var reloaded = await _context.SavedJobs
                .Include(s => s.Job)
                    .ThenInclude(j => j!.Product)
                .Include(s => s.Job)
                    .ThenInclude(j => j!.Employer)
                .Include(s => s.Job)
                    .ThenInclude(j => j!.JobType)
                .Include(s => s.Job)
                    .ThenInclude(j => j!.RequiredSkills)
                        .ThenInclude(rs => rs.Skill)
                .FirstOrDefaultAsync(s => s.SavedJobId == savedJob.SavedJobId);

            return _mapper.Map<SavedJobDto>(reloaded);
        }

        public async Task<bool> UnsaveJobAsync(Guid userId, Guid jobId)
        {
            var savedJob = await _context.SavedJobs
                .FirstOrDefaultAsync(s => s.UserId == userId && s.JobId == jobId);

            if (savedJob == null) return false;

            _context.SavedJobs.Remove(savedJob);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SavedJobDto>> GetUserSavedJobsAsync(Guid userId)
        {
            var savedJobs = await _context.SavedJobs
                .Include(s => s.Job)
                    .ThenInclude(j => j!.Product)
                        .ThenInclude(p => p!.Category)
                .Include(s => s.Job)
                    .ThenInclude(j => j!.Product)
                        .ThenInclude(p => p!.User)
                .Include(s => s.Job)
                    .ThenInclude(j => j!.Employer)
                .Include(s => s.Job)
                    .ThenInclude(j => j!.JobType)
                .Include(s => s.Job)
                    .ThenInclude(j => j!.CompanyProfile)
                .Include(s => s.Job)
                    .ThenInclude(j => j!.RequiredSkills)
                        .ThenInclude(rs => rs.Skill)
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.SavedAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<SavedJobDto>>(savedJobs);
        }

        public async Task<bool> IsJobSavedAsync(Guid userId, Guid jobId)
        {
            return await _context.SavedJobs.AnyAsync(s => s.UserId == userId && s.JobId == jobId);
        }
    }
}
