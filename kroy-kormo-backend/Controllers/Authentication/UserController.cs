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

        // Registratin user controller
        [HttpPost("register")]
        public async Task<ActionResult<ResponseDto<UserDto>>> Registration (UserRegistrationDto userDto)
        {
            var response =  await _userService.RegistrationAsync(userDto);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }

        // Login user controller
        [HttpPost("login")]
        public async Task<ActionResult<ResponseDto<UserDto>>> Login([FromBody] UserLoginDto loginDto)
        {
            var response = await _userService.LoginAsync(loginDto.UserName, loginDto.Password);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }

        // Get user by user id
        [HttpGet("{UserId}")]
        public async Task<ActionResult<ResponseDto<UserDto>>> GetUserById(Guid UserId)
        {
            var response = await _userService.GetUserById(UserId);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }

        [HttpPatch("{UserId}/password")]
        public async Task<ActionResult<ResponseDto<bool>>> UpdatePassword(Guid UserId, string OldPassword, string NewPassword)
        {
            var response = await _userService.UpdatePasswordAsync(UserId, OldPassword, NewPassword);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("{UserId}/profile")]
        public async Task<ActionResult<ResponseDto<UserDto>>> UpdateProfile(Guid UserId, UserDto userDto)
        {
            var response = await _userService.UpdateProfileAsync(UserId, userDto);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }
    }
}
