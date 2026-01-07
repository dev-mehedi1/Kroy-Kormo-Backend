using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Jobs;
using Sob_Need_Server.Services.Jobs;
using System.Security.Claims;

namespace Sob_Need_Server.Controllers.Jobs
{
    [ApiController]
    [Route("api/[controller]")]
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
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobDetail(Guid id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null) return NotFound();
            return Ok(job);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobListingDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var result = await _jobService.CreateJobAsync(userId, dto);
            return CreatedAtAction(nameof(GetJobDetail), new { id = result.JobId }, result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(Guid id, [FromBody] UpdateJobListingDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            try
            {
                var result = await _jobService.UpdateJobAsync(id, userId, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException) { return NotFound(); }
            catch (UnauthorizedAccessException) { return Forbid(); }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var success = await _jobService.DeleteJobAsync(id, userId);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet("my-jobs")]
        [Authorize]
        public async Task<IActionResult> GetMyJobs()
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var jobs = await _jobService.GetEmployerJobsAsync(userId);
            return Ok(jobs);
        }

        // Lookups
        [HttpGet("job-types")]
        public async Task<IActionResult> GetJobTypes() => Ok(await _jobService.GetJobTypesAsync());

        [HttpGet("experience-levels")]
        public async Task<IActionResult> GetExperienceLevels() => Ok(await _jobService.GetExperienceLevelsAsync());

        [HttpGet("skills")]
        public async Task<IActionResult> GetSkills([FromQuery] string? query) => Ok(await _jobService.GetSkillsAsync(query));
    }
}
