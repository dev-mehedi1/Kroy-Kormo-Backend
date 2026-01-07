using Sob_Need_Server.DTOs.User;

namespace Sob_Need_Server.Services.Jobs
{
    public interface IUserProfileService
    {
        // Public Profile
        Task<ExtendedProfileDto?> GetProfileByUserIdAsync(Guid userId);
        
        // Education
        Task<UserEducationDto> AddEducationAsync(Guid userId, UserEducationDto dto);
        Task<bool> UpdateEducationAsync(Guid userId, Guid educationId, UserEducationDto dto);
        Task<bool> DeleteEducationAsync(Guid userId, Guid educationId);
        
        // Experience
        Task<UserExperienceDto> AddExperienceAsync(Guid userId, UserExperienceDto dto);
        Task<bool> UpdateExperienceAsync(Guid userId, Guid experienceId, UserExperienceDto dto);
        Task<bool> DeleteExperienceAsync(Guid userId, Guid experienceId);
        
        // Skills
        Task<bool> AddSkillAsync(Guid userId, UserSkillDto dto);
        Task<bool> RemoveSkillAsync(Guid userId, Guid skillId);
        Task<bool> UpdateSkillProficiencyAsync(Guid userId, Guid skillId, string proficiency);
        
        // Resumes
        Task<UserResumeDto> UploadResumeAsync(Guid userId, UserResumeDto dto);
        Task<bool> SetDefaultResumeAsync(Guid userId, Guid resumeId);
        Task<bool> DeleteResumeAsync(Guid userId, Guid resumeId);
    }
}
