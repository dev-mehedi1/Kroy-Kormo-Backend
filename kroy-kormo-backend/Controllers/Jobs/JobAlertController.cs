using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Jobs;
using Sob_Need_Server.Services.Jobs;
using System.Security.Claims;

namespace Sob_Need_Server.Controllers.Jobs
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class JobAlertController : ControllerBase
    {
        private readonly IJobAlertService _jobAlertService;

        public JobAlertController(IJobAlertService jobAlertService)
        {
            _jobAlertService = jobAlertService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyAlerts()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);

            var alerts = await _jobAlertService.GetUserAlertsAsync(userId);
            return Ok(new ResponseDto<IEnumerable<JobAlertDto>>(true, "User alerts retrieved", alerts));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlert(Guid id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);

            var alert = await _jobAlertService.GetAlertByIdAsync(id, userId);
            if (alert == null) return NotFound(new ResponseDto<object>(false, "Alert not found", null!));
            
            return Ok(new ResponseDto<JobAlertDto>(true, "Alert details retrieved", alert));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlert([FromBody] CreateJobAlertDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);

            var result = await _jobAlertService.CreateAlertAsync(userId, dto);
            return CreatedAtAction(nameof(GetAlert), new { id = result.AlertId }, new ResponseDto<JobAlertDto>(true, "Alert created successfully", result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlert(Guid id, [FromBody] UpdateJobAlertDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);

            try
            {
                var result = await _jobAlertService.UpdateAlertAsync(id, userId, dto);
                return Ok(new ResponseDto<JobAlertDto>(true, "Alert updated successfully", result));
            }
            catch (KeyNotFoundException) { return NotFound(new ResponseDto<object>(false, "Alert not found", null!)); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlert(Guid id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);

            var success = await _jobAlertService.DeleteAlertAsync(id, userId);
            if (!success) return NotFound(new ResponseDto<object>(false, "Alert not found", null!));
            
            return Ok(new ResponseDto<object>(true, "Alert deleted successfully", null!));
        }

        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleAlert(Guid id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);

            var success = await _jobAlertService.ToggleAlertStatusAsync(id, userId);
            if (!success) return NotFound(new ResponseDto<object>(false, "Alert not found", null!));
            
            return Ok(new ResponseDto<object>(true, "Alert status toggled", null!));
        }

        [HttpGet("{id}/matches")]
        public async Task<IActionResult> GetAlertMatches(Guid id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);

            var matches = await _jobAlertService.GetAlertMatchesAsync(id, userId);
            return Ok(new ResponseDto<IEnumerable<JobListingDto>>(true, "Alert matches retrieved", matches));
        }
    }
}
