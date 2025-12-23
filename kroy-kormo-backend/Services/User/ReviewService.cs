using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.User;
using Sob_Need_Server.Interface.User;
using Sob_Need_Server.Models.Users;

namespace Sob_Need_Server.Services.User
{
    public class ReviewService(AppDbContext context, IMapper mapper) : IReviewService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseDto<ReviewDto>> AddReviewAsync(Guid reviewerId, ReviewDto reviewDto)
        {
            try
            {
                // Validate seller exists
                var sellerExists = await _context.Users.AnyAsync(u => u.UserId == reviewDto.SellerId);
                if (!sellerExists)
                    return new ResponseDto<ReviewDto>(false, "Seller not found", null!);

                // Check if reviewer already reviewed this seller
                var existingReview = await _context.Reviews
                    .FirstOrDefaultAsync(r => r.SellerId == reviewDto.SellerId && 
                                            r.ReviewerId == reviewerId &&
                                            r.ProductId == reviewDto.ProductId);

                if (existingReview != null)
                    return new ResponseDto<ReviewDto>(false, "You have already reviewed this seller for this product", null!);

                var review = new Review
                {
                    SellerId = reviewDto.SellerId,
                    ReviewerId = reviewerId,
                    ProductId = reviewDto.ProductId,
                    Rating = reviewDto.Rating,
                    Comment = reviewDto.Comment,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                var resultDto = _mapper.Map<ReviewDto>(review);
                return new ResponseDto<ReviewDto>(true, "Review added successfully", resultDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<ReviewDto>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<IEnumerable<ReviewDto>>> GetSellerReviewsAsync(Guid sellerId)
        {
            try
            {
                var reviews = await _context.Reviews
                    .Include(r => r.Seller)
                    .Include(r => r.Reviewer)
                    .Include(r => r.Product)
                    .Where(r => r.SellerId == sellerId && r.IsActive)
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync();

                var reviewDtos = _mapper.Map<IEnumerable<ReviewDto>>(reviews);
                return new ResponseDto<IEnumerable<ReviewDto>>(true, "Reviews retrieved successfully", reviewDtos);
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<ReviewDto>>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<double>> GetAverageRatingAsync(Guid sellerId)
        {
            try
            {
                var reviews = await _context.Reviews
                    .Where(r => r.SellerId == sellerId && r.IsActive)
                    .ToListAsync();

                if (!reviews.Any())
                    return new ResponseDto<double>(true, "No reviews found", 0);

                var averageRating = reviews.Average(r => r.Rating);
                return new ResponseDto<double>(true, "Average rating calculated", averageRating);
            }
            catch (Exception ex)
            {
                return new ResponseDto<double>(false, ex.Message, 0);
            }
        }
    }
}
