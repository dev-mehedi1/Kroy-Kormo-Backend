using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.User;
using Sob_Need_Server.Services.Jobs;
using System.Security.Claims;

namespace Sob_Need_Server.Controllers.Jobs
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _profileService;

        public UserProfileController(IUserProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyProfile()
        {
            var userIdStr = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            return Ok(new ResponseDto<ExtendedProfileDto>(true, "Profile retrieved", profile!));
        }

        [HttpGet("{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserProfile(Guid userId)
        {
            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile == null) return NotFound(new ResponseDto<object>(false, "Profile not found", null!));
            return Ok(new ResponseDto<ExtendedProfileDto>(true, "Profile retrieved", profile));
        }

        // Education
        [HttpPost("education")]
        public async Task<IActionResult> AddEducation([FromBody] UserEducationDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var result = await _profileService.AddEducationAsync(userId, dto);
            return Ok(new ResponseDto<UserEducationDto>(true, "Education added", result));
        }

        [HttpPut("education/{id}")]
        public async Task<IActionResult> UpdateEducation(Guid id, [FromBody] UserEducationDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var success = await _profileService.UpdateEducationAsync(userId, id, dto);
            return success ? Ok(new ResponseDto<object>(true, "Education updated", null!)) : NotFound(new ResponseDto<object>(false, "Education not found", null!));
        }

        [HttpDelete("education/{id}")]
        public async Task<IActionResult> DeleteEducation(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var success = await _profileService.DeleteEducationAsync(userId, id);
            return success ? Ok(new ResponseDto<object>(true, "Education deleted", null!)) : NotFound(new ResponseDto<object>(false, "Education not found", null!));
        }

        // Experience
        [HttpPost("experience")]
        public async Task<IActionResult> AddExperience([FromBody] UserExperienceDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var result = await _profileService.AddExperienceAsync(userId, dto);
            return Ok(new ResponseDto<UserExperienceDto>(true, "Experience added", result));
        }

        [HttpPut("experience/{id}")]
        public async Task<IActionResult> UpdateExperience(Guid id, [FromBody] UserExperienceDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var success = await _profileService.UpdateExperienceAsync(userId, id, dto);
            return success ? Ok(new ResponseDto<object>(true, "Experience updated", null!)) : NotFound(new ResponseDto<object>(false, "Experience not found", null!));
        }

        [HttpDelete("experience/{id}")]
        public async Task<IActionResult> DeleteExperience(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var success = await _profileService.DeleteExperienceAsync(userId, id);
            return success ? Ok(new ResponseDto<object>(true, "Experience deleted", null!)) : NotFound(new ResponseDto<object>(false, "Experience not found", null!));
        }

        // Skills
        [HttpPost("skill")]
        public async Task<IActionResult> AddSkill([FromBody] UserSkillDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            await _profileService.AddSkillAsync(userId, dto);
            return Ok(new ResponseDto<object>(true, "Skill added", null!));
        }

        [HttpDelete("skill/{skillId}")]
        public async Task<IActionResult> RemoveSkill(Guid skillId)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var success = await _profileService.RemoveSkillAsync(userId, skillId);
            return success ? Ok(new ResponseDto<object>(true, "Skill removed", null!)) : NotFound(new ResponseDto<object>(false, "Skill not found on profile", null!));
        }

        // Resumes
        [HttpPost("resume")]
        public async Task<IActionResult> UploadResume([FromBody] UserResumeDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var result = await _profileService.UploadResumeAsync(userId, dto);
            return Ok(new ResponseDto<UserResumeDto>(true, "Resume uploaded", result));
        }

        [HttpPatch("resume/{id}/default")]
        public async Task<IActionResult> SetDefaultResume(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var success = await _profileService.SetDefaultResumeAsync(userId, id);
            return success ? Ok(new ResponseDto<object>(true, "Default resume updated", null!)) : NotFound(new ResponseDto<object>(false, "Resume not found", null!));
        }

        [HttpDelete("resume/{id}")]
        public async Task<IActionResult> DeleteResume(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var success = await _profileService.DeleteResumeAsync(userId, id);
            return success ? Ok(new ResponseDto<object>(true, "Resume deleted", null!)) : NotFound(new ResponseDto<object>(false, "Resume not found", null!));
        }
    }
}
