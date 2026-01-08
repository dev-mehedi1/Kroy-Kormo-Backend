using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.User;
using Sob_Need_Server.Models.Users;
using Sob_Need_Server.Models.Jobs;

namespace Sob_Need_Server.Services.Jobs
{
    public class UserProfileService : IUserProfileService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserProfileService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ExtendedProfileDto?> GetProfileByUserIdAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Educations.OrderByDescending(e => e.EndDate))
                .Include(u => u.Experiences.OrderByDescending(e => e.EndDate))
                .Include(u => u.Skills).ThenInclude(s => s.Skill)
                .Include(u => u.Resumes)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            return _mapper.Map<ExtendedProfileDto>(user);
        }

        public async Task<UserEducationDto> AddEducationAsync(Guid userId, UserEducationDto dto)
        {
            var edu = _mapper.Map<UserEducation>(dto);
            edu.UserId = userId;
            if (edu.StartDate.HasValue) edu.StartDate = edu.StartDate.Value.ToUniversalTime();
            if (edu.EndDate.HasValue) edu.EndDate = edu.EndDate.Value.ToUniversalTime();
            _context.UserEducations.Add(edu);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserEducationDto>(edu);
        }

        public async Task<bool> UpdateEducationAsync(Guid userId, Guid educationId, UserEducationDto dto)
        {
            var edu = await _context.UserEducations.FirstOrDefaultAsync(e => e.EducationId == educationId && e.UserId == userId);
            if (edu == null) return false;

            _mapper.Map(dto, edu);
            if (edu.StartDate.HasValue) edu.StartDate = edu.StartDate.Value.ToUniversalTime();
            if (edu.EndDate.HasValue) edu.EndDate = edu.EndDate.Value.ToUniversalTime();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEducationAsync(Guid userId, Guid educationId)
        {
            var edu = await _context.UserEducations.FirstOrDefaultAsync(e => e.EducationId == educationId && e.UserId == userId);
            if (edu == null) return false;

            _context.UserEducations.Remove(edu);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserExperienceDto> AddExperienceAsync(Guid userId, UserExperienceDto dto)
        {
            var exp = _mapper.Map<UserExperience>(dto);
            exp.UserId = userId;
            if (exp.StartDate.HasValue) exp.StartDate = exp.StartDate.Value.ToUniversalTime();
            if (exp.EndDate.HasValue) exp.EndDate = exp.EndDate.Value.ToUniversalTime();
            _context.UserExperiences.Add(exp);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserExperienceDto>(exp);
        }

        public async Task<bool> UpdateExperienceAsync(Guid userId, Guid experienceId, UserExperienceDto dto)
        {
            var exp = await _context.UserExperiences.FirstOrDefaultAsync(e => e.ExperienceId == experienceId && e.UserId == userId);
            if (exp == null) return false;

            _mapper.Map(dto, exp);
            if (exp.StartDate.HasValue) exp.StartDate = exp.StartDate.Value.ToUniversalTime();
            if (exp.EndDate.HasValue) exp.EndDate = exp.EndDate.Value.ToUniversalTime();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteExperienceAsync(Guid userId, Guid experienceId)
        {
            var exp = await _context.UserExperiences.FirstOrDefaultAsync(e => e.ExperienceId == experienceId && e.UserId == userId);
            if (exp == null) return false;

            _context.UserExperiences.Remove(exp);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddSkillAsync(Guid userId, UserSkillDto dto)
        {
            var existing = await _context.UserSkills.AnyAsync(s => s.UserId == userId && s.SkillId == dto.SkillId);
            if (existing) return true;

            var userSkill = new UserSkill
            {
                UserId = userId,
                SkillId = dto.SkillId,
                YearsOfExperience = dto.YearsOfExperience,
                ProficiencyLevel = dto.ProficiencyLevel,
                CreatedAt = DateTime.UtcNow
            };

            _context.UserSkills.Add(userSkill);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveSkillAsync(Guid userId, Guid skillId)
        {
            var skill = await _context.UserSkills.FirstOrDefaultAsync(s => s.SkillId == skillId && s.UserId == userId);
            if (skill == null) return false;

            _context.UserSkills.Remove(skill);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateSkillProficiencyAsync(Guid userId, Guid skillId, string proficiency)
        {
            var skill = await _context.UserSkills.FirstOrDefaultAsync(s => s.SkillId == skillId && s.UserId == userId);
            if (skill == null) return false;

            skill.ProficiencyLevel = proficiency;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserResumeDto> UploadResumeAsync(Guid userId, UserResumeDto dto)
        {
            var resume = _mapper.Map<UserResume>(dto);
            resume.UserId = userId;
            
            // If first resume or set as default, unset others
            if (dto.IsDefault || !await _context.UserResumes.AnyAsync(r => r.UserId == userId))
            {
                var others = await _context.UserResumes.Where(r => r.UserId == userId).ToListAsync();
                foreach (var r in others) r.IsDefault = false;
                resume.IsDefault = true;
            }

            _context.UserResumes.Add(resume);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserResumeDto>(resume);
        }

        public async Task<bool> SetDefaultResumeAsync(Guid userId, Guid resumeId)
        {
            var resumes = await _context.UserResumes.Where(r => r.UserId == userId).ToListAsync();
            var target = resumes.FirstOrDefault(r => r.ResumeId == resumeId);
            if (target == null) return false;

            foreach (var r in resumes) r.IsDefault = (r.ResumeId == resumeId);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteResumeAsync(Guid userId, Guid resumeId)
        {
            var resume = await _context.UserResumes.FirstOrDefaultAsync(r => r.ResumeId == resumeId && r.UserId == userId);
            if (resume == null) return false;

            _context.UserResumes.Remove(resume);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
