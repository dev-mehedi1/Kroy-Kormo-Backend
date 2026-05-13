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
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            try
            {
                var result = await _applicationService.ApplyForJobAsync(userId, dto);
                return Ok(new ResponseDto<JobApplicationDto>(true, "Application submitted successfully", result));
            }
            catch (InvalidOperationException ex) { return BadRequest(new ResponseDto<object>(false, ex.Message, null!)); }
        }

        [HttpGet("my-applications")]
        public async Task<IActionResult> GetMyApplications([FromQuery] ApplicationFilterDto filter)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var result = await _applicationService.GetUserApplicationsAsync(userId, filter);
            return Ok(new ResponseDto<PaginatedResponseDto<JobApplicationDto>>(true, "User applications retrieved", result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplication(Guid id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var app = await _applicationService.GetApplicationDetailAsync(id, userId);
            if (app == null) return NotFound(new ResponseDto<object>(false, "Application not found", null!));
            return Ok(new ResponseDto<JobApplicationDto>(true, "Application details retrieved", app));
        }

        [HttpPost("{id}/withdraw")]
        public async Task<IActionResult> Withdraw(Guid id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var success = await _applicationService.WithdrawApplicationAsync(id, userId);
            return success ? Ok(new ResponseDto<object>(true, "Application withdrawn", null!)) : NotFound(new ResponseDto<object>(false, "Application not found", null!));
        }

        // Employer endpoints
        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetApplicationsForJob(Guid jobId, [FromQuery] ApplicationFilterDto filter)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var result = await _applicationService.GetJobApplicationsAsync(jobId, userId, filter);
            return Ok(new ResponseDto<PaginatedResponseDto<JobApplicationDto>>(true, "Job applications retrieved", result));
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var success = await _applicationService.UpdateApplicationStatusAsync(id, userId, dto.StatusId, dto.Notes);
            return success ? Ok(new ResponseDto<object>(true, "Status updated successfully", null!)) : NotFound(new ResponseDto<object>(false, "Application not found", null!));
        }

        [HttpGet("{id}/history")]
        public async Task<IActionResult> GetHistory(Guid id)
        {
            var result = await _applicationService.GetApplicationHistoryAsync(id);
            return Ok(new ResponseDto<IEnumerable<ApplicationStatusHistoryDto>>(true, "Application history retrieved", result));
        }
    }

    public class UpdateStatusDto
    {
        public Guid StatusId { get; set; }
        public string? Notes { get; set; }
    }
}
