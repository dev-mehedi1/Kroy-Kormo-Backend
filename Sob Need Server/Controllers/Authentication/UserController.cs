using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Authentication;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface.Authentication;
using Sob_Need_Server.Utils;

namespace Sob_Need_Server.Controllers.Authentication
{
    [Route(Constants.BasePath + "/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        [HttpPost]
        public async Task<ActionResult<ResponseDto<UserDto>>> Registration (UserDto userDto)
        {
            var response =  await _userService.RegistrationAsync(userDto);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }

        
    }
}
