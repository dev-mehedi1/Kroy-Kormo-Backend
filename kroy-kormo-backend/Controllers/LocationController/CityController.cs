using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Location;
using Sob_Need_Server.Interface.Location;
using Sob_Need_Server.Utils;
using DTOs.Location.Filter;

namespace Sob_Need_Server.Controllers.LocationController;

[Route(Constants.BasePath + "/[controller]")]
[ApiController]
public class CityController(ICityServices cityService) : ControllerBase
{
    private readonly ICityServices _cityService = cityService;

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateCity ([FromBody] CityDto cityDto)
    {
        var response = await _cityService.CreateAsync(cityDto);
        if (response.IsSuccess) return Ok(response);
        return BadRequest(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetCities([FromQuery] CityFilterDto filterDto)
    {
        var response = await _cityService.GetAllAsync(filterDto);
        if (response.IsSuccess) return Ok(response);
        return BadRequest(response);
    }
}
