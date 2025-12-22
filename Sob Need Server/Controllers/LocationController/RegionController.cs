using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Location;
using Sob_Need_Server.DTOs.Location.Filter;
using Sob_Need_Server.Interface.Location;
using Sob_Need_Server.Utils;

namespace Sob_Need_Server.Controllers.LocationController
{
    [Route(Constants.BasePath + "/[controller]")]
    [ApiController]
    public class RegionController(IRegionServices regionService) : ControllerBase
    {
        private readonly IRegionServices _regionService = regionService;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateRegion ([FromBody] RegionDto regionDto)
        {
            var response = await _regionService.CreateAsync(regionDto);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetRegions([FromQuery] RegionFilterDto filterDto)
        {
            var response = await _regionService.GetAllAsync(filterDto);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }
    }
}
