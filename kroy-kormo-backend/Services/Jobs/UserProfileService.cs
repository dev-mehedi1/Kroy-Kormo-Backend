using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.User;
using Sob_Need_Server.Models.Jobs;
using Sob_Need_Server.Models.Users;
using Sob_Need_Server.Services.Storage;

namespace Sob_Need_Server.Services.Jobs
{
    public class UserProfileService : IUserProfileService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;
        private readonly ICVGenerationService _cvGenerationService;

        public UserProfileService(
            AppDbContext context, 
            IMapper mapper, 
            IFileUploadService fileUploadService,
            ICVGenerationService cvGenerationService)
        {
            _context = context;
            _mapper = mapper;
            _fileUploadService = fileUploadService;
            _cvGenerationService = cvGenerationService;
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

        public async Task<UserResumeDto> UploadResumeAsync(Guid userId, IFormFile file, string? title = null)
        {
            var options = new FileUploadOptions
            {
                AllowedExtensions = new[] { "pdf" },
                MaxFileSizeMB = 5
            };

            var uploadResult = await _fileUploadService.UploadAsync(file, options);
            if (!uploadResult.Success)
            {
                throw new Exception(uploadResult.ErrorMessage ?? "File upload failed");
            }

            var resume = new UserResume
            {
                UserId = userId,
                FileName = uploadResult.FileName,
                FileUrl = uploadResult.FileUrl,
                FileId = uploadResult.FileId,
                FileSize = uploadResult.FileSize,
                FileType = "pdf",
                ResumeTitle = title ?? Path.GetFileNameWithoutExtension(file.FileName),
                IsDefault = !await _context.UserResumes.AnyAsync(r => r.UserId == userId),
                UploadedAt = DateTime.UtcNow
            };

            _context.UserResumes.Add(resume);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserResumeDto>(resume);
        }

        public async Task<UserResumeDto> GenerateAndUploadCVAsync(Guid userId)
        {
            var pdfBytes = await _cvGenerationService.GeneratePdfCVAsync(userId);
            var fileName = $"CV_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
            
            using var stream = new MemoryStream(pdfBytes);
            var file = new FormFile(stream, 0, pdfBytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            return await UploadResumeAsync(userId, file, "Online Generated CV");
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

        public async Task<ExtendedProfileDto> UpdateProfileAsync(Guid userId, UpdateProfileDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) throw new Exception("User not found");

            // Update allowed fields
            if (dto.Bio != null) user.Bio = dto.Bio;
            if (dto.AboutMe != null) user.AboutMe = dto.AboutMe;
            if (dto.CurrentSalary.HasValue) user.CurrentSalary = dto.CurrentSalary;
            if (dto.ExpectedSalary.HasValue) user.ExpectedSalary = dto.ExpectedSalary;
            if (dto.IsSalaryVisible.HasValue) user.IsSalaryVisible = dto.IsSalaryVisible.Value;
            if (dto.IsFresher.HasValue) user.IsFresher = dto.IsFresher.Value;
            
            await _context.SaveChangesAsync();
            return _mapper.Map<ExtendedProfileDto>(user);
        }

    }
}
