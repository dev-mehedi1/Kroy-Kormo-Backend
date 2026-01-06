using Sob_Need_Server.DTOs.Common;
using DTOs.Product;

namespace Sob_Need_Server.Interface
{
    public interface ISuggestionService
    {
        Task<ResponseDto<IEnumerable<ProductDto>>> GetRecommendedAdsAsync(Guid userId, int count = 10);
        Task<ResponseDto<IEnumerable<ProductDto>>> GetNearYouAsync(Guid userId, double? lat, double? lon, int count = 10);
        Task<ResponseDto<IEnumerable<ProductDto>>> GetTrendingAsync(Guid userId, int count = 10);
        Task<ResponseDto<IEnumerable<ProductDto>>> GetRecentlyViewedSimilarAsync(Guid userId, int count = 10);
    }
}
