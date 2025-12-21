using Sob_Need_Server.DTOs.Authentication;
using Sob_Need_Server.DTOs.Common;

namespace Sob_Need_Server.Interface.Authentication
{
    public interface IUserService
    {
        public Task<ResponseDto<UserDto>> RegistrationAsync(UserRegistrationDto entity);
        public Task<ResponseDto<UserDto>> LoginAsync(string UserName, string Password);
        public Task<ResponseDto<UserDto>> GetUserById(Guid UserId);
    }
}

