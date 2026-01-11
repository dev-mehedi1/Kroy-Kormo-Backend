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
    public class RecruitmentController : ControllerBase
    {
        private readonly IRecruitmentService _recruitmentService;

        public RecruitmentController(IRecruitmentService recruitmentService)
        {
            _recruitmentService = recruitmentService;
        }

        // Company Profile
        [HttpGet("company/my")]
        public async Task<IActionResult> GetMyCompanies()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var result = await _recruitmentService.GetUserCompanyProfilesAsync(userId);
            return Ok(new ResponseDto<IEnumerable<CompanyProfileDto>>(true, "Company profiles retrieved", result));
        }

        [HttpPost("company")]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyProfileDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var result = await _recruitmentService.CreateCompanyProfileAsync(userId, dto);
            return Ok(new ResponseDto<CompanyProfileDto>(true, "Company profile created", result));
        }

        [HttpPut("company")]
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyProfileDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var result = await _recruitmentService.UpdateCompanyProfileAsync(userId, dto);
            return Ok(new ResponseDto<CompanyProfileDto>(true, "Company profile updated", result));
        }

        [HttpGet("company/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var result = await _recruitmentService.GetCompanyProfileAsync(id);
            return Ok(new ResponseDto<CompanyProfileDto>(true, "Company profile retrieved", result!));
        }

        // Interviews
        [HttpPost("interview")]
        public async Task<IActionResult> ScheduleInterview([FromBody] InterviewDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var result = await _recruitmentService.ScheduleInterviewAsync(userId, dto);
            return Ok(new ResponseDto<InterviewDto>(true, "Interview scheduled", result));
        }

        [HttpPatch("interview/{id}/status")]
        public async Task<IActionResult> UpdateInterviewStatus(Guid id, [FromQuery] string status)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var success = await _recruitmentService.UpdateInterviewStatusAsync(id, userId, status);
            return success ? Ok(new ResponseDto<object>(true, "Interview status updated", null!)) : NotFound(new ResponseDto<object>(false, "Interview not found", null!));
        }

        [HttpGet("job/{jobId}/interviews")]
        public async Task<IActionResult> GetJobInterviews(Guid jobId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var result = await _recruitmentService.GetJobInterviewsAsync(jobId, userId);
            return Ok(new ResponseDto<IEnumerable<InterviewDto>>(true, "Job interviews retrieved", result));
        }

        [HttpGet("interviews/my")]
        public async Task<IActionResult> GetMyInterviews()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var result = await _recruitmentService.GetMyInterviewsAsync(userId);
            return Ok(new ResponseDto<IEnumerable<InterviewDto>>(true, "All interviews retrieved", result));
        }

        [HttpGet("interviews/candidate/my")]
        public async Task<IActionResult> GetMyCandidateInterviews()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var result = await _recruitmentService.GetMyCandidateInterviewsAsync(userId);
            return Ok(new ResponseDto<IEnumerable<InterviewDto>>(true, "My interviews retrieved", result));
        }

        // Offers
        [HttpPost("offer")]
        public async Task<IActionResult> CreateOffer([FromBody] JobOfferDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var result = await _recruitmentService.CreateOfferAsync(userId, dto);
            return Ok(new ResponseDto<JobOfferDto>(true, "Offer extended successfully", result));
        }

        [HttpPost("offer/{id}/respond")]
        public async Task<IActionResult> RespondToOffer(Guid id, [FromBody] OfferResponseDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var success = await _recruitmentService.RespondToOfferAsync(id, userId, dto.Response, dto.Notes);
            return success ? Ok(new ResponseDto<object>(true, "Response recorded", null!)) : NotFound(new ResponseDto<object>(false, "Offer not found", null!));
        }

        // Stats
        [HttpGet("job/{jobId}/stats")]
        public async Task<IActionResult> GetJobStats(Guid jobId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);
            var result = await _recruitmentService.GetJobStatisticsAsync(jobId, userId);
            return Ok(new ResponseDto<JobStatisticsDto>(true, "Job statistics retrieved", result));
        }
    }

    public class OfferResponseDto
    {
        public string Response { get; set; } = null!;
        public string? Notes { get; set; }
    }
}
