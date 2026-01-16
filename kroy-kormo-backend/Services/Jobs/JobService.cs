using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.Products;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Jobs;
using Sob_Need_Server.Models.Jobs;
using Sob_Need_Server.Models.Products;

namespace Sob_Need_Server.Services.Jobs
{
    public class JobService : IJobService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public JobService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<JobListingDto> CreateJobAsync(Guid employerId, CreateJobListingDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Create Product Entry
                var product = new Product
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    CategoryId = dto.CategoryId,
                    UserId = employerId,
                    IsJobAd = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    Slug = GenerateSlug(dto.Title) // Utility or manual for now
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                // 2. Create JobListing Entry
                var jobListing = _mapper.Map<JobListing>(dto);
                jobListing.ProductId = product.ProductId;
                jobListing.EmployerId = employerId;
                jobListing.JobStatus = "Active";
                jobListing.CreatedAt = DateTime.UtcNow;

                _context.JobListings.Add(jobListing);
                await _context.SaveChangesAsync();

                // 3. Handle Skills
                if (dto.SkillIds != null && dto.SkillIds.Any())
                {
                    foreach (var skillId in dto.SkillIds)
                    {
                        var jobSkill = new JobSkill
                        {
                            JobId = jobListing.JobId,
                            SkillId = skillId,
                            IsRequired = true // Default for now
                        };
                        _context.JobSkills.Add(jobSkill);
                    }
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return await GetJobByIdAsync(jobListing.JobId) ?? throw new Exception("Failed to retrieve created job");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<JobListingDto> UpdateJobAsync(Guid jobId, Guid employerId, UpdateJobListingDto dto)
        {
            var job = await _context.JobListings
                .Include(j => j.Product)
                .FirstOrDefaultAsync(j => j.JobId == jobId && j.EmployerId == employerId);

            if (job == null) throw new KeyNotFoundException("Job not found or unauthorized");

            _mapper.Map(dto, job);
            
            if (job.Product != null)
            {
                job.Product.Title = dto.Title;
                job.Product.Description = dto.Description;
                job.Product.CategoryId = dto.CategoryId;
                job.Product.UpdatedAt = DateTime.UtcNow;
            }

            job.UpdatedAt = DateTime.UtcNow;

            // Handle Skills Update (Simple: clear and re-add)
            var existingSkills = _context.JobSkills.Where(js => js.JobId == jobId);
            _context.JobSkills.RemoveRange(existingSkills);

            if (dto.SkillIds != null && dto.SkillIds.Any())
            {
                foreach (var skillId in dto.SkillIds)
                {
                    _context.JobSkills.Add(new JobSkill { JobId = jobId, SkillId = skillId });
                }
            }

            await _context.SaveChangesAsync();
            return await GetJobByIdAsync(jobId) ?? throw new Exception("Failed to retrieve updated job");
        }

        public async Task<bool> DeleteJobAsync(Guid jobId, Guid employerId)
        {
            var job = await _context.JobListings
                .Include(j => j.Product)
                .FirstOrDefaultAsync(j => j.JobId == jobId && j.EmployerId == employerId);

            if (job == null) return false;

            // Soft delete or hard delete? Let's do hard delete for now but deactivate product
            job.IsActive = false;
            if (job.Product != null) job.Product.IsActive = false;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<JobListingDto?> GetJobByIdAsync(Guid jobId)
        {
            var job = await _context.JobListings
                .Include(j => j.Product)
                .Include(j => j.Employer)
                .Include(j => j.JobType)
                .Include(j => j.ExperienceLevel)
                .Include(j => j.Category)
                .Include(j => j.CompanyProfile)
                .Include(j => j.RequiredSkills).ThenInclude(rs => rs.Skill)
                .FirstOrDefaultAsync(j => j.JobId == jobId && j.IsActive);

            return _mapper.Map<JobListingDto>(job);
        }

        public async Task<PaginatedResponseDto<JobListingDto>> SearchJobsAsync(JobSearchFilter filter)
        {
            var query = _context.JobListings
                .Include(j => j.Product)
                .Include(j => j.Employer)
                .Include(j => j.JobType)
                .Include(j => j.ExperienceLevel)
                .Include(j => j.Category)
                .Include(j => j.CompanyProfile)
                .Where(j => j.IsActive && j.JobStatus == "Active")
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                var keyword = filter.Keyword.ToLower();

                query = query.Where(j =>
                    j.Product!.Title.ToLower().Contains(keyword) ||
                    j.Product!.Description.ToLower().Contains(keyword)
                );
            }

            if (filter.CategoryId.HasValue)
                query = query.Where(j => j.CategoryId == filter.CategoryId.Value);

            if (filter.JobTypeId.HasValue)
                query = query.Where(j => j.JobTypeId == filter.JobTypeId.Value);

            if (filter.ExperienceLevelId.HasValue)
                query = query.Where(j => j.ExperienceLevelId == filter.ExperienceLevelId.Value);

            if (filter.IsRemote.HasValue)
                query = query.Where(j => j.IsRemote == filter.IsRemote.Value);

            if (filter.MinSalary.HasValue)
                query = query.Where(j => j.SalaryMax >= filter.MinSalary.Value);

            if (filter.MaxSalary.HasValue)
                query = query.Where(j => j.SalaryMin <= filter.MaxSalary.Value);

            if (filter.SkillIds != null && filter.SkillIds.Any())
                query = query.Where(j => j.RequiredSkills.Any(rs => filter.SkillIds.Contains(rs.SkillId)));

            if (filter.DaysAgo.HasValue)
            {
                var cutoff = DateTime.UtcNow.AddDays(-filter.DaysAgo.Value);
                query = query.Where(j => j.CreatedAt >= cutoff);
            }

            if (filter.CompanyId.HasValue)
                query = query.Where(j => j.CompanyProfileId == filter.CompanyId.Value);

            // Sorting
            query = filter.SortBy?.ToLower() switch
            {
                "salary_desc" => query.OrderByDescending(j => j.SalaryMax),
                "salary_asc" => query.OrderBy(j => j.SalaryMin),
                "mostviewed" => query.OrderByDescending(j => j.ViewCount),
                "mostapplied" => query.OrderByDescending(j => j.ApplicationCount),
                _ => query.OrderByDescending(j => j.CreatedAt)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<JobListingDto>>(items);

            return new PaginatedResponseDto<JobListingDto>
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

        public async Task<PaginatedResponseDto<JobListingDto>> GetEmployerJobsAsync(Guid employerId, JobSearchFilter filter)
        {
            var query = _context.JobListings
                .Include(j => j.Product)
                .Include(j => j.JobType)
                .Include(j => j.ExperienceLevel)
                .Where(j => j.EmployerId == employerId && j.IsActive)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(j => j.Product!.Title.Contains(filter.Keyword) || j.Product!.Description.Contains(filter.Keyword));
            }

            query = filter.SortBy?.ToLower() switch
            {
                "oldest" => query.OrderBy(j => j.CreatedAt),
                _ => query.OrderByDescending(j => j.CreatedAt)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<JobListingDto>>(items);

            return new PaginatedResponseDto<JobListingDto>
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

        public async Task<bool> UpdateJobStatusAsync(Guid jobId, Guid employerId, string status)
        {
            var job = await _context.JobListings.FirstOrDefaultAsync(j => j.JobId == jobId && j.EmployerId == employerId);
            if (job == null) return false;

            job.JobStatus = status;
            job.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleRemoteStatusAsync(Guid jobId, Guid employerId)
        {
            var job = await _context.JobListings.FirstOrDefaultAsync(j => j.JobId == jobId && j.EmployerId == employerId);
            if (job == null) return false;

            job.IsRemote = !job.IsRemote;
            job.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<JobTypeDto>> GetJobTypesAsync()
        {
            var types = await _context.JobTypes.Where(t => t.IsActive).ToListAsync();
            return _mapper.Map<IEnumerable<JobTypeDto>>(types);
        }

        public async Task<IEnumerable<ExperienceLevelDto>> GetExperienceLevelsAsync()
        {
            var levels = await _context.ExperienceLevels.Where(l => l.IsActive).ToListAsync();
            return _mapper.Map<IEnumerable<ExperienceLevelDto>>(levels);
        }

        public async Task<IEnumerable<SkillDto>> GetSkillsAsync(string? query)
        {
            var skillsQuery = _context.Skills.Where(s => s.IsActive);
            if (!string.IsNullOrEmpty(query))
                skillsQuery = skillsQuery.Where(s => s.SkillName.Contains(query));

            var skills = await skillsQuery.Take(20).ToListAsync();
            return _mapper.Map<IEnumerable<SkillDto>>(skills);
        }

        private string GenerateSlug(string title)
        {
            return title.ToLower().Replace(" ", "-") + "-" + Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
