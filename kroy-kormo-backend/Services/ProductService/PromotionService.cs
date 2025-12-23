using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface.Products;
using Sob_Need_Server.Models.Products;
using AutoMapper;
using Sob_Need_Server.DTOs.Product;

namespace Sob_Need_Server.Services.ProductService
{
    public class PromotionService(AppDbContext context, IMapper mapper) : IPromotionService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseDto<IEnumerable<PromotionDto>>> GetActivePromotionsAsync(Guid productId)
        {
            try
            {
                var promotions = await _context.ProductPromotions
                    .Where(p => p.ProductId == productId && p.IsActive && p.EndDate >= DateTime.UtcNow)
                    .ToListAsync();
                
                var dtos = _mapper.Map<IEnumerable<PromotionDto>>(promotions);
                return new ResponseDto<IEnumerable<PromotionDto>>(true, "Promotions retrieved", dtos);
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<PromotionDto>>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<PromotionDto>> ApplyPromotionAsync(Guid productId, string promotionType, int durationDays, Guid userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null) return new ResponseDto<PromotionDto>(false, "Product not found", null!);

                // Security check
                if (product.UserId != userId)
                {
                    return new ResponseDto<PromotionDto>(false, "You do not have permission to apply promotions to this product", null!);
                }

                // Deactivate existing promotion of same type if any
                var existing = await _context.ProductPromotions
                    .Where(p => p.ProductId == productId && p.PromotionType == promotionType && p.IsActive)
                    .FirstOrDefaultAsync();
                
                if (existing != null)
                {
                    existing.IsActive = false;
                }

                var promotion = new ProductPromotion
                {
                    ProductId = productId,
                    PromotionType = promotionType,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(durationDays),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.ProductPromotions.Add(promotion);
                await _context.SaveChangesAsync();
                
                // Update product flags using centralized logic
                await UpdateProductPromotionFlags(productId);

                await transaction.CommitAsync();

                var dto = _mapper.Map<PromotionDto>(promotion);
                return new ResponseDto<PromotionDto>(true, "Promotion applied successfully", dto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResponseDto<PromotionDto>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<bool>> DeactivatePromotionAsync(Guid promotionId, Guid userId)
        {
            try
            {
                var promotion = await _context.ProductPromotions
                    .Include(p => p.Product)
                    .FirstOrDefaultAsync(p => p.PromotionId == promotionId);

                if (promotion == null) return new ResponseDto<bool>(false, "Promotion not found", false);
                
                // Security check
                if (promotion.Product?.UserId != userId)
                {
                    return new ResponseDto<bool>(false, "You do not have permission to deactivate this promotion", false);
                }

                promotion.IsActive = false;
                await _context.SaveChangesAsync();

                // Re-evaluate product flags
                await UpdateProductPromotionFlags(promotion.ProductId);

                return new ResponseDto<bool>(true, "Promotion deactivated", true);
            }
            catch (Exception ex)
            {
                return new ResponseDto<bool>(false, ex.Message, false);
            }
        }

        private async Task UpdateProductPromotionFlags(Guid productId)
        {
            var product = await _context.Products
                .Include(p => p.Promotions)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null) return;

            var activePromotions = product.Promotions
                .Where(p => p.IsActive && p.EndDate >= DateTime.UtcNow)
                .ToList();

            product.IsUrgentAd = activePromotions.Any(p => p.PromotionType == "Urgent");
            product.IsTopAd = activePromotions.Any(p => p.PromotionType == "TopAd");
            product.IsFeaturedMember = activePromotions.Any(p => p.PromotionType == "Featured");

            await _context.SaveChangesAsync();
        }
    }
}
