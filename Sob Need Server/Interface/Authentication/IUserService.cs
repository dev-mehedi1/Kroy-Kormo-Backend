using Sob_Need_Server.DTOs.Authentication;
using Sob_Need_Server.DTOs.Common;

namespace Sob_Need_Server.Interface.Authentication
{
    public interface IUserService
    {
        public Task<ResponseDto<UserDto>> RegistrationAsync(UserDto entity);
    }
}
