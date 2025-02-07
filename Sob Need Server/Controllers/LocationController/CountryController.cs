using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Location;
using Sob_Need_Server.Interface.Location;
using Sob_Need_Server.Utils;

namespace Sob_Need_Server.Controllers.LocationController
{
    [Route(Constants.BasePath + "/[controller]")]
    [ApiController]
    public class CountryController(ICountryService countryService) : ControllerBase
    {
        private readonly ICountryService _countryService = countryService;
        [HttpPost]
        public async Task<ActionResult<ResponseDto<CountryDto>>> CreateProduct ([FromBody] CountryDto countryDto)
        {
            var response = await _countryService.CreateAsync(countryDto);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }
    }
}
