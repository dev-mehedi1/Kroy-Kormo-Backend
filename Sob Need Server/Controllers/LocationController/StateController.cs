using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Location;
using Sob_Need_Server.DTOs.Location.Filter;
using Sob_Need_Server.Interface.Location;
using Sob_Need_Server.Utils;

namespace Sob_Need_Server.Controllers.LocationController
{
    [Route(Constants.BasePath + "/[controller]")]
    [ApiController]
    public class StateController(IStateServices stateService) : ControllerBase
    {
        private readonly IStateServices _stateService = stateService;

        [HttpPost]
        public async Task<IActionResult> CreateState ([FromBody] StateDto StateDto)
        {
            var response = await _stateService.CreateAsync(StateDto);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetStates([FromQuery] StateFilterDto filterDto)
        {
            var response = await _stateService.GetAllAsync(filterDto);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }
    }
}
