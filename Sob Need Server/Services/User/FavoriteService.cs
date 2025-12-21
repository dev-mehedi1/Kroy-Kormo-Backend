using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.User;
using Sob_Need_Server.Interface.User;
using Sob_Need_Server.Models.Users;

namespace Sob_Need_Server.Services.User
{
    public class FavoriteService(AppDbContext context, IMapper mapper) : IFavoriteService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseDto<FavoriteDto>> AddFavoriteAsync(Guid userId, Guid productId)
        {
            try
            {
                // Check if already favorited
                var existingFavorite = await _context.Favorites
                    .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

                if (existingFavorite != null)
                    return new ResponseDto<FavoriteDto>(false, "Product already in favorites", null!);

                // Check if product exists
                var productExists = await _context.Products.AnyAsync(p => p.ProductId == productId);
                if (!productExists)
                    return new ResponseDto<FavoriteDto>(false, "Product not found", null!);

                var favorite = new Favorite
                {
                    UserId = userId,
                    ProductId = productId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Favorites.Add(favorite);

                // Update product favorite count
                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    product.FavoriteCount++;
                    _context.Products.Update(product);
                }

                await _context.SaveChangesAsync();

                var favoriteDto = _mapper.Map<FavoriteDto>(favorite);
                return new ResponseDto<FavoriteDto>(true, "Added to favorites successfully", favoriteDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<FavoriteDto>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<bool>> RemoveFavoriteAsync(Guid userId, Guid productId)
        {
            try
            {
                var favorite = await _context.Favorites
                    .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

                if (favorite == null)
                    return new ResponseDto<bool>(false, "Favorite not found", false);

                _context.Favorites.Remove(favorite);

                // Update product favorite count
                var product = await _context.Products.FindAsync(productId);
                if (product != null && product.FavoriteCount > 0)
                {
                    product.FavoriteCount--;
                    _context.Products.Update(product);
                }

                await _context.SaveChangesAsync();

                return new ResponseDto<bool>(true, "Removed from favorites successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseDto<bool>(false, ex.Message, false);
            }
        }

        public async Task<ResponseDto<IEnumerable<FavoriteDto>>> GetUserFavoritesAsync(Guid userId)
        {
            try
            {
                var favorites = await _context.Favorites
                    .Include(f => f.Product)
                        .ThenInclude(p => p!.Category)
                    .Include(f => f.Product)
                        .ThenInclude(p => p!.Location)
                    .Where(f => f.UserId == userId)
                    .OrderByDescending(f => f.CreatedAt)
                    .ToListAsync();

                var favoriteDtos = _mapper.Map<IEnumerable<FavoriteDto>>(favorites);
                return new ResponseDto<IEnumerable<FavoriteDto>>(true, "Favorites retrieved successfully", favoriteDtos);
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<FavoriteDto>>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<bool>> IsFavoritedAsync(Guid userId, Guid productId)
        {
            try
            {
                var isFavorited = await _context.Favorites
                    .AnyAsync(f => f.UserId == userId && f.ProductId == productId);

                return new ResponseDto<bool>(true, "Check completed", isFavorited);
            }
            catch (Exception ex)
            {
                return new ResponseDto<bool>(false, ex.Message, false);
            }
        }
    }
}
