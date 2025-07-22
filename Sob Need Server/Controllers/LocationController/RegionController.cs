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
    public class CityController(ICityServices CityService) : ControllerBase
    {
        private readonly ICityServices _CityService = CityService;

        [HttpPost]
        public async Task<IActionResult> CreateCity ([FromBody] CityDto CityDto)
        {
            var response = await _CityService.CreateAsync(CityDto);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCitys([FromQuery] CityFilterDto filterDto)
        {
            var response = await _CityService.GetAllAsync(filterDto);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }
    }
}
