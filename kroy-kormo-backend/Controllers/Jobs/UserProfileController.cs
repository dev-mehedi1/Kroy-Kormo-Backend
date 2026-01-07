using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.User;
using Sob_Need_Server.Services.Jobs;
using System.Security.Claims;

namespace Sob_Need_Server.Controllers.Jobs
{
    [ApiController]
    [Route("api/[controller]")]
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
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            return Ok(profile);
        }

        [HttpGet("{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserProfile(Guid userId)
        {
            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        // Education
        [HttpPost("education")]
        public async Task<IActionResult> AddEducation([FromBody] UserEducationDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            return Ok(await _profileService.AddEducationAsync(userId, dto));
        }

        [HttpPut("education/{id}")]
        public async Task<IActionResult> UpdateEducation(Guid id, [FromBody] UserEducationDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var success = await _profileService.UpdateEducationAsync(userId, id, dto);
            return success ? Ok() : NotFound();
        }

        [HttpDelete("education/{id}")]
        public async Task<IActionResult> DeleteEducation(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var success = await _profileService.DeleteEducationAsync(userId, id);
            return success ? NoContent() : NotFound();
        }

        // Experience
        [HttpPost("experience")]
        public async Task<IActionResult> AddExperience([FromBody] UserExperienceDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            return Ok(await _profileService.AddExperienceAsync(userId, dto));
        }

        [HttpPut("experience/{id}")]
        public async Task<IActionResult> UpdateExperience(Guid id, [FromBody] UserExperienceDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var success = await _profileService.UpdateExperienceAsync(userId, id, dto);
            return success ? Ok() : NotFound();
        }

        [HttpDelete("experience/{id}")]
        public async Task<IActionResult> DeleteExperience(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var success = await _profileService.DeleteExperienceAsync(userId, id);
            return success ? NoContent() : NotFound();
        }

        // Skills
        [HttpPost("skill")]
        public async Task<IActionResult> AddSkill([FromBody] UserSkillDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            await _profileService.AddSkillAsync(userId, dto);
            return Ok();
        }

        [HttpDelete("skill/{skillId}")]
        public async Task<IActionResult> RemoveSkill(Guid skillId)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var success = await _profileService.RemoveSkillAsync(userId, skillId);
            return success ? NoContent() : NotFound();
        }

        // Resumes
        [HttpPost("resume")]
        public async Task<IActionResult> UploadResume([FromBody] UserResumeDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            return Ok(await _profileService.UploadResumeAsync(userId, dto));
        }

        [HttpPatch("resume/{id}/default")]
        public async Task<IActionResult> SetDefaultResume(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var success = await _profileService.SetDefaultResumeAsync(userId, id);
            return success ? Ok() : NotFound();
        }

        [HttpDelete("resume/{id}")]
        public async Task<IActionResult> DeleteResume(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var success = await _profileService.DeleteResumeAsync(userId, id);
            return success ? NoContent() : NotFound();
        }
    }
}
