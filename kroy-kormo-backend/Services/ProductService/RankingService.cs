using Microsoft.EntityFrameworkCore;
using Models.Products;
using Sob_Need_Server.Context;
using Sob_Need_Server.Interface;

namespace Sob_Need_Server.Services.ProductService
{
    public class RankingService(AppDbContext context) : IRankingService
    {
        private readonly AppDbContext _context = context;

        // Scoring Constants
        private const decimal BaseScoreDefault = 1000m;
        private const decimal TopAdBoost = 5000m;
        private const decimal UrgentAdBoost = 2000m;
        private const decimal FeaturedAdBoost = 1000m;
        
        // Decay Rate: Points lost per hour
        private const decimal DecayRatePerHour = 10m; 

        public decimal CalculateScore(Product product)
        {
            decimal score = product.BaseScore > 0 ? product.BaseScore : BaseScoreDefault;

            // Apply Active Boosts
            // Verify if promotions are actually active by date? 
            // Assuming IsTopAd/IsUrgentAd/IsFeaturedMember flags are maintained up-to-date by PromotionService 
            // or we check active promotions here. 
            // For performance, relying on flags is better if they are reliable. 
            // But let's check Start/End date if promotions are loaded to be precise, 
            // otherwise fallback to flags.

            bool isTop = product.IsTopAd;
            bool isUrgent = product.IsUrgentAd;
            bool isFeatured = product.IsFeaturedMember;

            if (product.Promotions != null && product.Promotions.Count != 0)
            {
                var now = DateTime.UtcNow;
                var activePromotions = product.Promotions
                    .Where(p => p.IsActive && p.StartDate <= now && p.EndDate >= now)
                    .ToList();

                if (activePromotions.Any(p => p.PromotionType == "TopAd")) isTop = true;
                if (activePromotions.Any(p => p.PromotionType == "Urgent")) isUrgent = true; // or "Urgent" matches string
                if (activePromotions.Any(p => p.PromotionType == "Featured")) isFeatured = true;
            }

            if (isTop) score += TopAdBoost;
            if (isUrgent) score += UrgentAdBoost;
            if (isFeatured) score += FeaturedAdBoost;

            // Freshness / Decay
            // Use LastBumpedAt if available, otherwise CreatedAt
            var referenceTime = product.LastBumpedAt ?? product.CreatedAt;
            var hoursElapsed = (decimal)(DateTime.UtcNow - referenceTime).TotalHours;

            // Decay Score
            decimal decay = hoursElapsed * DecayRatePerHour;
            score -= decay;

            // Ensure score doesn't drop below zero (optional, but good for sanity)
            return Math.Max(0, score);
        }

        public async Task UpdateProductScoreAsync(Guid productId)
        {
            var product = await _context.Products
                .Include(p => p.Promotions)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product != null)
            {
                product.FinalScore = CalculateScore(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAllScoresAsync()
        {
            // Process in batches to avoid locking table for too long
            int batchSize = 500;
            int page = 0;
            bool hasMore = true;

            while (hasMore)
            {
                var products = await _context.Products
                    .Include(p => p.Promotions)
                    .Where(p => p.IsActive && p.IsSell) // Only active sell ads need ranking usually
                    .OrderBy(p => p.ProductId)
                    .Skip(page * batchSize)
                    .Take(batchSize)
                    .ToListAsync();

                if (products.Count == 0)
                {
                    hasMore = false;
                    break;
                }

                foreach (var product in products)
                {
                    // Sync Flags with Active Promotions
                    var now = DateTime.UtcNow;
                    bool isTop = false;
                    bool isUrgent = false;
                    bool isFeatured = false;

                    if (product.Promotions != null)
                    {
                        var activePromotions = product.Promotions
                            .Where(p => p.IsActive && p.StartDate <= now && p.EndDate >= now)
                            .ToList();

                        if (activePromotions.Any(p => p.PromotionType == "TopAd")) isTop = true;
                        if (activePromotions.Any(p => p.PromotionType == "Urgent")) isUrgent = true;
                        if (activePromotions.Any(p => p.PromotionType == "Featured")) isFeatured = true;
                    }

                    // Update flags if changed (Logic to auto-expire)
                    if (product.IsTopAd != isTop) product.IsTopAd = isTop;
                    if (product.IsUrgentAd != isUrgent) product.IsUrgentAd = isUrgent;
                    if (product.IsFeaturedMember != isFeatured) product.IsFeaturedMember = isFeatured;

                    product.FinalScore = CalculateScore(product);
                }

                await _context.SaveChangesAsync();
                page++;
            }
        }
    }
}
