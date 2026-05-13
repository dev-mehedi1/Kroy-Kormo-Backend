namespace Sob_Need_Server.Services.Jobs
{
    public interface IProfileValidationService
    {
        /// <summary>
        /// Validate if user profile is complete enough for job applications
        /// </summary>
        /// <param name="userId">User ID to validate</param>
        /// <returns>Validation result with missing fields</returns>
        Task<ProfileValidationResult> ValidateProfileForJobApplicationAsync(Guid userId);
    }
    
    public class ProfileValidationResult
    {
        public bool IsComplete { get; set; }
        public List<string> MissingFields { get; set; } = new();
        public string Message { get; set; } = null!;
    }
}
