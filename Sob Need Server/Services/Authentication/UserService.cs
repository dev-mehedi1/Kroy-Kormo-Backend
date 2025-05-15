using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Authentication;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface.Authentication;
using Sob_Need_Server.Models.Authentication;
using Sob_Need_Server.Utils.Helpers;

namespace Sob_Need_Server.Services.Authentication;

public class UserService (AppDbContext dbContext, IConfiguration configuration, TokenHelper tokenHelper, IMapper mapper) : IUserService
{
    public readonly AppDbContext _context = dbContext;
    public readonly IMapper _mapper = mapper;
    public readonly IConfiguration _configuration = configuration;
    private readonly TokenHelper _tokenHelper = tokenHelper;

    public async Task<ResponseDto<UserDto>> GetUserById(Guid UserId)
    {
        try
        {
            var isUser = await _context.Users.AnyAsync(u => u.UserId == UserId);
            if (!isUser) return new ResponseDto<UserDto>(false, "User Not Found", null!);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == UserId);
            user.Password = null!;
            var userDto = _mapper.Map<UserDto>(user);

            return new ResponseDto<UserDto>(true, "User Retriving Successful", userDto!);
        }
        catch (Exception ex)
        {
            return new ResponseDto<UserDto>(false, ex.Message, null!);
        }
    }

    public async Task<ResponseDto<UserDto>> LoginAsync(string UserName, string Password)
    {
        try
        {
            // CHeck User is exist
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == UserName);
            if (user == null) return new ResponseDto<UserDto>(false, "User not exist using this user name", null!);

            // Verify password
            var isPasswordValid = PasswordHelper.VerifyPassword(user.Password ,Password);
            if (!isPasswordValid) return new ResponseDto<UserDto>(false, "Invalid password", null!);

            // Generate JWT token
            var token = _tokenHelper.GenerateJwtToken(user);
            // Map user to UserDto
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Token = token;
            return new ResponseDto<UserDto>(true, "Login successful", userDto);
        }
        catch (Exception ex)
        {
            return new ResponseDto<UserDto>(false, ex.Message, null!);
        }
    }

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
