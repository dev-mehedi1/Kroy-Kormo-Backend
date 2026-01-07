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
    public class RecruitmentController : ControllerBase
    {
        private readonly IRecruitmentService _recruitmentService;

        public RecruitmentController(IRecruitmentService recruitmentService)
        {
            _recruitmentService = recruitmentService;
        }

        // Company Profile
        [HttpGet("company/my")]
        public async Task<IActionResult> GetMyCompany()
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            return Ok(await _recruitmentService.GetUserCompanyProfileAsync(userId));
        }

        [HttpPost("company")]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyProfileDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            return Ok(await _recruitmentService.CreateCompanyProfileAsync(userId, dto));
        }

        [HttpPut("company")]
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyProfileDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            return Ok(await _recruitmentService.UpdateCompanyProfileAsync(userId, dto));
        }

        [HttpGet("company/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            return Ok(await _recruitmentService.GetCompanyProfileAsync(id));
        }

        // Interviews
        [HttpPost("interview")]
        public async Task<IActionResult> ScheduleInterview([FromBody] InterviewDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            return Ok(await _recruitmentService.ScheduleInterviewAsync(userId, dto));
        }

        [HttpPatch("interview/{id}/status")]
        public async Task<IActionResult> UpdateInterviewStatus(Guid id, [FromQuery] string status)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var success = await _recruitmentService.UpdateInterviewStatusAsync(id, userId, status);
            return success ? Ok() : NotFound();
        }

        [HttpGet("job/{jobId}/interviews")]
        public async Task<IActionResult> GetJobInterviews(Guid jobId)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            return Ok(await _recruitmentService.GetJobInterviewsAsync(jobId, userId));
        }

        // Offers
        [HttpPost("offer")]
        public async Task<IActionResult> CreateOffer([FromBody] JobOfferDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            return Ok(await _recruitmentService.CreateOfferAsync(userId, dto));
        }

        [HttpPost("offer/{id}/respond")]
        public async Task<IActionResult> RespondToOffer(Guid id, [FromBody] OfferResponseDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var success = await _recruitmentService.RespondToOfferAsync(id, userId, dto.Response, dto.Notes);
            return success ? Ok() : NotFound();
        }

        // Stats
        [HttpGet("job/{jobId}/stats")]
        public async Task<IActionResult> GetJobStats(Guid jobId)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            return Ok(await _recruitmentService.GetJobStatisticsAsync(jobId, userId));
        }
    }

    public class OfferResponseDto
    {
        public string Response { get; set; } = null!;
        public string? Notes { get; set; }
    }
}
