using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.User;

namespace Sob_Need_Server.Interface.User
{
    public interface IReviewService
    {
        Task<ResponseDto<ReviewDto>> AddReviewAsync(Guid reviewerId, ReviewDto reviewDto);
        Task<ResponseDto<IEnumerable<ReviewDto>>> GetSellerReviewsAsync(Guid sellerId);
        Task<ResponseDto<double>> GetAverageRatingAsync(Guid sellerId);
    }
}
