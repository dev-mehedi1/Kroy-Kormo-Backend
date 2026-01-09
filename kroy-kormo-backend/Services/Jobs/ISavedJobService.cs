using Sob_Need_Server.DTOs.Jobs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sob_Need_Server.Services.Jobs
{
    public interface ISavedJobService
    {
        Task<SavedJobDto> SaveJobAsync(Guid userId, CreateSavedJobDto dto);
        Task<bool> UnsaveJobAsync(Guid userId, Guid jobId);
        Task<IEnumerable<SavedJobDto>> GetUserSavedJobsAsync(Guid userId);
        Task<bool> IsJobSavedAsync(Guid userId, Guid jobId);
    }
}
