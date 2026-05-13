using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;

namespace Sob_Need_Server.Services.Jobs
{
    public class ProfileValidationService : IProfileValidationService
    {
        private readonly AppDbContext _context;
        
        public ProfileValidationService(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<ProfileValidationResult> ValidateProfileForJobApplicationAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Educations.Where(e => e.IsActive))
                .Include(u => u.Skills)
                .FirstOrDefaultAsync(u => u.UserId == userId);
            
            if (user == null)
            {
                return new ProfileValidationResult
                {
                    IsComplete = false,
                    Message = "User not found",
                    MissingFields = new List<string> { "User" }
                };
            }
            
            var missingFields = new List<string>();
            
            // Check About Me (required)
            if (string.IsNullOrWhiteSpace(user.AboutMe))
            {
                missingFields.Add("About Me / Profile Summary");
            }
            
            // Check Education (at least one required)
            if (!user.Educations.Any())
            {
                missingFields.Add("Education");
            }
            
            // Check Skills (at least one required)
            if (!user.Skills.Any())
            {
                missingFields.Add("Skills");
            }
            
            // Experience is NOT required (can be fresher)
            // Salary is NOT required (optional)
            
            var isComplete = !missingFields.Any();
            
            return new ProfileValidationResult
            {
                IsComplete = isComplete,
                MissingFields = missingFields,
                Message = isComplete 
                    ? "Profile is complete" 
                    : $"Please complete the following required fields: {string.Join(", ", missingFields)}"
            };
        }
    }
}
