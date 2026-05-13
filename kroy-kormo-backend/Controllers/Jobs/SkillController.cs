using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Jobs;
using Sob_Need_Server.Interface.Jobs;
using Sob_Need_Server.Utils;

namespace Sob_Need_Server.Controllers.Jobs
{
    [Route(Constants.BasePath + "/skills")]
    [ApiController] 
    public class SkillController(ISkillService skillService) : ControllerBase
    {
        private readonly ISkillService _skillService = skillService;

        [HttpGet]
        public async Task<IActionResult> GetSkills([FromQuery] string? search, [FromQuery] Guid? categoryId)
        {
            var response = await _skillService.GetAllAsync(search, categoryId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSkill(Guid id)
        {
            var response = await _skillService.GetByIdAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin,Owner")]
        public async Task<IActionResult> CreateSkill([FromBody] SkillDto skillDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseDto<SkillDto>(false, "Invalid Data", null!));

            var response = await _skillService.CreateAsync(skillDto);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,Admin,Owner")]
        public async Task<IActionResult> UpdateSkill(Guid id, [FromBody] SkillDto skillDto)
        {
            var response = await _skillService.UpdateAsync(id, skillDto);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,Admin,Owner")]
        public async Task<IActionResult> DeleteSkill(Guid id)
        {
             var response = await _skillService.DeleteAsync(id);
             return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
