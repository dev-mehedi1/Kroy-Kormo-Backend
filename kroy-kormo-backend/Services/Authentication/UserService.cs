using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Authentication;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface.Authentication;
using Sob_Need_Server.Models.Authentication;
using Sob_Need_Server.Utils.Helpers;
using Microsoft.AspNetCore.SignalR;
using Sob_Need_Server.SignalR;

namespace Sob_Need_Server.Services.Authentication;

public class UserService (AppDbContext dbContext, IConfiguration configuration, TokenHelper tokenHelper, IMapper mapper, IHubContext<NotificationHub> hubContext) : IUserService
{
    public readonly AppDbContext _context = dbContext;
    public readonly IMapper _mapper = mapper;
    public readonly IConfiguration _configuration = configuration;
    private readonly TokenHelper _tokenHelper = tokenHelper;
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;

    public async Task<ResponseDto<UserDto>> GetUserById(Guid UserId)
    {
        try
        {
            var isUser = await _context.Users.AnyAsync(u => u.UserId == UserId);
            if (!isUser) return new ResponseDto<UserDto>(false, "User Not Found", null!);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == UserId);
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
            var user = await _context.Users
                .Include(u => u.UserType)
                .FirstOrDefaultAsync(x => x.UserName == UserName);
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

    public async Task<ResponseDto<UserDto>> RegistrationAsync(UserRegistrationDto entity)
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

            // Map UserRegistrationDto to User model
            var user = _mapper.Map<Models.Authentication.User>(entity);
            user.Password = hashPassword;
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

    public async Task<ResponseDto<bool>> UpdatePasswordAsync(Guid UserId, string OldPassword, string NewPassword)
    {
        try
        {
            var user = await _context.Users.FindAsync(UserId);
            if (user == null) return new ResponseDto<bool>(false, "User not found", false);

            var isPasswordValid = PasswordHelper.VerifyPassword(user.Password, OldPassword);
            if (!isPasswordValid) return new ResponseDto<bool>(false, "Invalid old password", false);

            user.Password = PasswordHelper.GenerateHashedPassword(NewPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new ResponseDto<bool>(true, "Password updated successfully", true);
        }
        catch (Exception ex)
        {
            return new ResponseDto<bool>(false, ex.Message, false);
        }
    }

    public async Task<ResponseDto<UserDto>> UpdateProfileAsync(Guid UserId, UserDto entity)
    {
        try
        {
            var user = await _context.Users.FindAsync(UserId);
            if (user == null) return new ResponseDto<UserDto>(false, "User not found", null!);

            user.FullName = entity.FullName ?? user.FullName;
            user.Phone = entity.Phone ?? user.Phone;
            user.Bio = entity.Bio ?? user.Bio;
            user.Address = entity.Address ?? user.Address;
            user.ProfilePicture = entity.ProfilePicture ?? user.ProfilePicture;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);
            
            // Broadcast to other sessions of the same user
            await _hubContext.Clients.User(UserId.ToString()).SendAsync("UpdateProfileTelemetry", userDto);

            return new ResponseDto<UserDto>(true, "Profile updated successfully", userDto);
        }
        catch (Exception ex)
        {
            return new ResponseDto<UserDto>(false, ex.Message, null!);
        }
    }
}
