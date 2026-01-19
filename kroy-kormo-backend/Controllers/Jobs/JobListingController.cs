using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Jobs;
using Sob_Need_Server.Services.Jobs;
using System.Security.Claims;

namespace Sob_Need_Server.Controllers.Jobs
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class JobListingController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobListingController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<IActionResult> SearchJobs([FromQuery] JobSearchFilter filter)
        {
            var result = await _jobService.SearchJobsAsync(filter);
            return Ok(new ResponseDto<IEnumerable<JobListingDto>>(true, "Jobs retrieved successfully", result.Data, result.TotalCount));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobDetail(Guid id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null) return NotFound(new ResponseDto<object>(false, "Job not found", null!));
            return Ok(new ResponseDto<JobListingDto>(true, "Job details retrieved", job));
        }

        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetJobBySlug(string slug)
        {
            var job = await _jobService.GetJobBySlugAsync(slug);
            if (job == null) return NotFound(new ResponseDto<object>(false, "Job not found", null!));
            return Ok(new ResponseDto<JobListingDto>(true, "Job details retrieved", job));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobListingDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var result = await _jobService.CreateJobAsync(userId, dto);
            return CreatedAtAction(nameof(GetJobDetail), new { id = result.JobId }, new ResponseDto<JobListingDto>(true, "Job created successfully", result));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(Guid id, [FromBody] UpdateJobListingDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            try
            {
                var result = await _jobService.UpdateJobAsync(id, userId, dto);
                return Ok(new ResponseDto<JobListingDto>(true, "Job updated successfully", result));
            }
            catch (KeyNotFoundException) { return NotFound(new ResponseDto<object>(false, "Job not found", null!)); }
            catch (UnauthorizedAccessException) { return Forbid(); }
        }

        [Authorize]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateJobStatus(Guid id, [FromBody] string status)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);

            var success = await _jobService.UpdateJobStatusAsync(id, userId, status);
            if (!success) return NotFound(new ResponseDto<object>(false, "Job not found or unauthorized", null!));
            
            return Ok(new ResponseDto<object>(true, "Job status updated successfully", null!));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(Guid id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var success = await _jobService.DeleteJobAsync(id, userId);
            if (!success) return NotFound(new ResponseDto<object>(false, "Job not found", null!));
            return Ok(new ResponseDto<object>(true, "Job deleted successfully", null!));
        }

        [HttpGet("my-jobs")]
        [Authorize]
        public async Task<IActionResult> GetMyJobs([FromQuery] JobSearchFilter filter)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var result = await _jobService.GetEmployerJobsAsync(userId, filter);
            return Ok(new ResponseDto<PaginatedResponseDto<JobListingDto>>(true, "Employer jobs retrieved", result));
        }

        // Lookups
        [HttpGet("job-types")]
        public async Task<IActionResult> GetJobTypes() => Ok(new ResponseDto<IEnumerable<JobTypeDto>>(true, "Job types retrieved", await _jobService.GetJobTypesAsync()));
        
        [HttpGet("experience-levels")]
        public async Task<IActionResult> GetExperienceLevels() => Ok(new ResponseDto<IEnumerable<ExperienceLevelDto>>(true, "Experience levels retrieved", await _jobService.GetExperienceLevelsAsync()));

        [HttpGet("skills")]
        public async Task<IActionResult> GetSkills([FromQuery] string? query) => Ok(new ResponseDto<IEnumerable<SkillDto>>(true, "Skills retrieved", await _jobService.GetSkillsAsync(query)));
    }
}
