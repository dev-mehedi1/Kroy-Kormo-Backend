using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Authentication;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface.Authentication;
using Sob_Need_Server.Models.Authentication;
using Sob_Need_Server.Utils.Helpers;

namespace Sob_Need_Server.Services.Authentication
{
    public class UserService (AppDbContext dbContext, IConfiguration configuration, IMapper mapper) : IUserService
    {
        public readonly AppDbContext _context = dbContext;
        public readonly IMapper _mapper = mapper;
        public readonly IConfiguration _configuration = configuration;

        public async Task<ResponseDto<UserDto>> RegistrationAsync(UserDto entity)
        {
           try
            {
                // Check if email or username already exists
                var checkEmail = await _context.Users.AnyAsync(x => x.Email == entity.Email);
                if (checkEmail) return new ResponseDto<UserDto>(false, "Email Already Used", null!);
                var checkUserName = await _context.Users.AnyAsync(x => x.UserName == entity.UserName);
                if (checkUserName) return new ResponseDto<UserDto>(false, "User Name Already Used", null!);

                // Hash Password
                var hashPassword = PasswordHelper.GenerateHashedPassword(entity.Password);
                entity.Password = hashPassword;

                // Map UserDto to User model
                var user = _mapper.Map<User>(entity);
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                // Map saved user back to UserDto
                var userDto = _mapper.Map<UserDto>(user);
                return new ResponseDto<UserDto>(true, "User registered successfully", userDto);

            }
            catch (Exception ex)
            {
                return new ResponseDto<UserDto>(false , $"{ex.Message}" , null!);
            }
        }
    }
}
