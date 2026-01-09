using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Jobs;
using Sob_Need_Server.Services.Jobs;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sob_Need_Server.Controllers.Jobs
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SavedJobController : ControllerBase
    {
        private readonly ISavedJobService _savedJobService;

        public SavedJobController(ISavedJobService savedJobService)
        {
            _savedJobService = savedJobService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMySavedJobs()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId")!);
            var savedJobs = await _savedJobService.GetUserSavedJobsAsync(userId);
            return Ok(savedJobs);
        }

        [HttpPost]
        public async Task<IActionResult> SaveJob(CreateSavedJobDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId")!);
            var result = await _savedJobService.SaveJobAsync(userId, dto);
            return Ok(result);
        }

        [HttpDelete("{jobId}")]
        public async Task<IActionResult> UnsaveJob(Guid jobId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId")!);
            var result = await _savedJobService.UnsaveJobAsync(userId, jobId);
            if (!result) return NotFound();
            return Ok(new { message = "Job removed from saved list" });
        }

        [HttpGet("check/{jobId}")]
        public async Task<IActionResult> CheckIsSaved(Guid jobId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId")!);
            var isSaved = await _savedJobService.IsJobSavedAsync(userId, jobId);
            return Ok(isSaved);
        }
    }
}
