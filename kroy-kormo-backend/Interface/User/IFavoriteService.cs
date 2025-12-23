using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.User;

namespace Sob_Need_Server.Interface.User
{
    public interface IFavoriteService
    {
        Task<ResponseDto<FavoriteDto>> AddFavoriteAsync(Guid userId, Guid productId);
        Task<ResponseDto<bool>> RemoveFavoriteAsync(Guid userId, Guid productId);
        Task<ResponseDto<IEnumerable<FavoriteDto>>> GetUserFavoritesAsync(Guid userId);
        Task<ResponseDto<bool>> IsFavoritedAsync(Guid userId, Guid productId);
    }
}
