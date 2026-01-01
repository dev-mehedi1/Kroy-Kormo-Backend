using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.Interface;
using Sob_Need_Server.Utils;

namespace Sob_Need_Server.Controllers.Product
{
    [Route(Constants.BasePath + "/[controller]")]
    [ApiController]
    public class ConditionController(IConditionService conditionService) : ControllerBase
    {
        private readonly IConditionService _conditionService = conditionService;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCondition([FromBody] ConditionDto conditionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto<ConditionDto>(false, "Invalid request data.", null!));
            }

            var response = await _conditionService.CreateAsync(conditionDto);

            if (response.IsSuccess)
            {
                return CreatedAtAction(nameof(GetConditionById), new { id = response.Data?.ConditionId }, response);
            }

            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetConditions([FromQuery] ConditionFilterDto filter)
        {
            var response = await _conditionService.GetAllAsync(filter);
            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConditionById(Guid id)
        {
            var response = await _conditionService.GetByIdAsync(id);
            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCondition(Guid id, [FromBody] ConditionDto conditionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto<ConditionDto>(false, "Invalid request data.", null!));
            }

            var response = await _conditionService.UpdateAsync(id, conditionDto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCondition(Guid id)
        {
            var response = await _conditionService.DeleteAsync(id);
            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
