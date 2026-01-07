using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Jobs;
using Sob_Need_Server.Services.Jobs;
using System.Security.Claims;

namespace Sob_Need_Server.Controllers.Jobs
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JobApplicationController : ControllerBase
    {
        private readonly IJobApplicationService _applicationService;

        public JobApplicationController(IJobApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpPost]
        public async Task<IActionResult> Apply([FromBody] CreateApplicationDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            try
            {
                var result = await _applicationService.ApplyForJobAsync(userId, dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("my-applications")]
        public async Task<IActionResult> GetMyApplications()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(await _applicationService.GetUserApplicationsAsync(userId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplication(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var app = await _applicationService.GetApplicationDetailAsync(id, userId);
            if (app == null) return NotFound();
            return Ok(app);
        }

        [HttpPost("{id}/withdraw")]
        public async Task<IActionResult> Withdraw(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var success = await _applicationService.WithdrawApplicationAsync(id, userId);
            return success ? Ok() : NotFound();
        }

        // Employer endpoints
        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetApplicationsForJob(Guid jobId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(await _applicationService.GetJobApplicationsAsync(jobId, userId));
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var success = await _applicationService.UpdateApplicationStatusAsync(id, userId, dto.StatusId, dto.Notes);
            return success ? Ok() : NotFound();
        }

        [HttpGet("{id}/history")]
        public async Task<IActionResult> GetHistory(Guid id)
        {
            return Ok(await _applicationService.GetApplicationHistoryAsync(id));
        }
    }

    public class UpdateStatusDto
    {
        public Guid StatusId { get; set; }
        public string? Notes { get; set; }
    }
}
