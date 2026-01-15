using Sob_Need_Server.DTOs.User;

namespace Sob_Need_Server.Services.Jobs
{
    public interface ICVGenerationService
    {
        Task<byte[]> GeneratePdfCVAsync(Guid userId);
    }
}
