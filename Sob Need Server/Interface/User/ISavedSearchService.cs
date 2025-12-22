using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.User;

namespace Sob_Need_Server.Interface.User
{
    public interface ISavedSearchService
    {
        Task<ResponseDto<SavedSearchDto>> SaveSearchAsync(Guid userId, SavedSearchDto savedSearchDto);
        Task<ResponseDto<IEnumerable<SavedSearchDto>>> GetSavedSearchesAsync(Guid userId);
        Task<ResponseDto<bool>> DeleteSavedSearchAsync(Guid searchId, Guid userId);
    }
}
