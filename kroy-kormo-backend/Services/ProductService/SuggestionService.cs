using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Common;
using DTOs.Product;
using Sob_Need_Server.Interface;

namespace Services.ProductService
{
    public class SuggestionService(AppDbContext context, IMapper mapper, IMemoryCache cache) : ISuggestionService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _cache = cache;

        // Weights
        private const int WeightView = 1;
        private const int WeightFavorite = 5;
        // private const int WeightMessage = 10; // Not implemented yet

        private const string CacheKeyPrefix = "Suggestions_";

        public async Task<ResponseDto<IEnumerable<ProductDto>>> GetRecommendedAdsAsync(Guid userId, int count = 10)
        {
            var cacheKey = $"{CacheKeyPrefix}Rec_{userId}";
            if (_cache.TryGetValue(cacheKey, out IEnumerable<ProductDto>? cachedSuggestions))
            {
                return new ResponseDto<IEnumerable<ProductDto>>(true, "Retrieved from cache", cachedSuggestions!);
            }

            try
            {
                // 1. Fetch User Signals (Last 30 days)
                var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
                
                var recentViews = await _context.ProductViews
                    .Where(pv => pv.UserId == userId && pv.ViewedAt >= thirtyDaysAgo)
                    .Select(pv => new { pv.ProductId, pv.ViewedAt })
                    .ToListAsync();

                var recentFavorites = await _context.Favorites
                    .Where(f => f.UserId == userId && f.CreatedAt >= thirtyDaysAgo)
                    .Select(f => new { f.ProductId, f.CreatedAt })
                    .ToListAsync();

                if (recentViews.Count == 0 && recentFavorites.Count == 0) 
                {
                    // Fallback to trending if no history
                    return await GetTrendingAsync(userId, count);
                }

                // 2. Extract Interests (Categories, Tags logic can be added later if Tags exist)
                var productIds = recentViews.Select(v => v.ProductId).Union(recentFavorites.Select(f => f.ProductId)).Distinct().ToList();

                var referenceProducts = await _context.Products
                    .Where(p => productIds.Contains(p.ProductId))
                    .Select(p => new { p.ProductId, p.CategoryId })
                    .ToListAsync();

                // Calculate Category Scores
                var categoryScores = new Dictionary<Guid, int>();

                foreach (var refProd in referenceProducts)
                {
                    int score = 0;
                    score += recentViews.Count(v => v.ProductId == refProd.ProductId) * WeightView;
                    score += recentFavorites.Count(f => f.ProductId == refProd.ProductId) * WeightFavorite;

                    if (!categoryScores.ContainsKey(refProd.CategoryId)) categoryScores[refProd.CategoryId] = 0;
                    categoryScores[refProd.CategoryId] += score;
                }

                var topCategories = categoryScores.OrderByDescending(x => x.Value).Take(3).Select(x => x.Key).ToList();

                // 3. Find Candidates
                var candidates = await _context.Products
                    .Include(p => p.User)
                    .Include(p => p.Category)
                    .Include(p => p.Condition)
                    .Include(p => p.Status)
                    .Include(p => p.Locations).ThenInclude(l => l.City)
                    .Include(p => p.Attachments)
                    .Where(p => topCategories.Contains(p.CategoryId) 
                                && p.UserId != userId // Exclude own ads
                                && p.IsActive 
                                && p.IsSell) 
                    .OrderByDescending(p => p.FinalScore) // Initial Ranking
                    .Take(count * 5) // Fetch pool
                    .ToListAsync();

                // 4. Refine Ranking (Mix FinalScore + Interest Similarity)
                // Since we filtered by Top Category, they already match interest. 
                // We can just rely on FinalScore which includes Freshness and Boosts.
                // To avoid same seller domination:
                var distinctSellerCandidates = candidates
                    .GroupBy(p => p.UserId)
                    .SelectMany(g => g.Take(2)) // Max 2 per seller
                    .OrderByDescending(p => p.FinalScore)
                    .Take(count)
                    .ToList();

                var result = _mapper.Map<IEnumerable<ProductDto>>(distinctSellerCandidates);

                // Cache for 10 minutes
                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));

                return new ResponseDto<IEnumerable<ProductDto>>(true, "Recommended ads retrieved", result);
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<ProductDto>>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<IEnumerable<ProductDto>>> GetNearYouAsync(Guid userId, double? lat, double? lon, int count = 10)
        {
             // For now, since we don't have Geo-Spatial DB enabled or wired up, 
             // we will try to infer location from User's most viewed city or just return global trending if fails.
             // Ideally this uses PostGIS (Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite).
             
             // Simple Logic: Find products in the user's most interacted Cities.
             
            try
            {
                 var recentViews = await _context.ProductViews
                    .Where(pv => pv.UserId == userId)
                    .OrderByDescending(pv => pv.ViewedAt)
                    .Take(20)
                    .Select(pv => pv.ProductId)
                    .ToListAsync();

                 List<Guid> targetCityIds = [];
                 
                 if (recentViews.Count != 0)
                 {
                     var cities = await _context.Locations
                         .Where(l => recentViews.Contains(l.ProductId) && l.CityId.HasValue)
                         .GroupBy(l => l.CityId)
                         .OrderByDescending(g => g.Count())
                         .Select(g => g.Key!.Value)
                         .Take(3)
                         .ToListAsync();
                     
                     targetCityIds.AddRange(cities);
                 }

                 if (targetCityIds.Count == 0)
                 {
                     // Fallback: Try to find any active city from products? Or just return top ads.
                     return await GetTrendingAsync(userId, count);
                 }

                 var products = await _context.Products
                    .Include(p => p.User)
                    .Include(p => p.Category)
                    .Include(p => p.Condition)
                    .Include(p => p.Status)
                    .Include(p => p.Locations).ThenInclude(l => l.City)
                    .Include(p => p.Attachments)
                    .Where(p => p.IsActive 
                                && p.IsSell 
                                && p.UserId != userId
                                && p.Locations.Any(l => l.CityId.HasValue && targetCityIds.Contains(l.CityId.Value)))
                    .OrderByDescending(p => p.FinalScore)
                    .Take(count)
                    .ToListAsync();

                 var result = _mapper.Map<IEnumerable<ProductDto>>(products);
                 return new ResponseDto<IEnumerable<ProductDto>>(true, "Nearby ads retrieved", result);
            }
            catch (Exception ex)
            {
                 return new ResponseDto<IEnumerable<ProductDto>>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<IEnumerable<ProductDto>>> GetTrendingAsync(Guid userId, int count = 10)
        {
            var cacheKey = $"{CacheKeyPrefix}Trending";
            if (_cache.TryGetValue(cacheKey, out IEnumerable<ProductDto>? cachedSuggestions))
            {
                return new ResponseDto<IEnumerable<ProductDto>>(true, "Retrieved from cache", cachedSuggestions!);
            }

            try
            {
                // High FinalScore usually implies Trending (Recent + Boosted), but let's add ViewCount weight explicitly
                var products = await _context.Products
                    .Include(p => p.User)
                    .Include(p => p.Category)
                    .Include(p => p.Condition)
                    .Include(p => p.Status)
                    .Include(p => p.Locations).ThenInclude(l => l.City)
                    .Include(p => p.Attachments)
                    .Where(p => p.IsActive && p.IsSell && p.UserId != userId)
                    .OrderByDescending(p => p.FinalScore + p.ViewCount * 0.5m) // Simple formula
                    .Take(count)
                    .ToListAsync();
                
                var result = _mapper.Map<IEnumerable<ProductDto>>(products);
                
                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(15)); // Longer cache for trending

                return new ResponseDto<IEnumerable<ProductDto>>(true, "Trending ads retrieved", result);
            }
            catch (Exception ex)
            {
                 return new ResponseDto<IEnumerable<ProductDto>>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<IEnumerable<ProductDto>>> GetRecentlyViewedSimilarAsync(Guid userId, int count = 10)
        {
            try
            {
                var lastView = await _context.ProductViews
                    .Where(pv => pv.UserId == userId)
                    .OrderByDescending(pv => pv.ViewedAt)
                    .Include(pv => pv.Product)
                    .FirstOrDefaultAsync();

                if (lastView == null || lastView.Product == null)
                {
                    return new ResponseDto<IEnumerable<ProductDto>>(true, "No recent history", []);
                }

                var targetCategory = lastView.Product.CategoryId;
                var excludeId = lastView.ProductId;

                var products = await _context.Products
                    .Include(p => p.User)
                    .Include(p => p.Category)
                    .Include(p => p.Condition)
                    .Include(p => p.Status)
                    .Include(p => p.Locations).ThenInclude(l => l.City)
                    .Include(p => p.Attachments)
                    .Where(p => p.IsActive 
                                && p.IsSell 
                                && p.CategoryId == targetCategory 
                                && p.ProductId != excludeId
                                && p.UserId != userId)
                    .OrderByDescending(p => p.FinalScore) // Rank by score
                    .Take(count)
                    .ToListAsync();

                 var result = _mapper.Map<IEnumerable<ProductDto>>(products);
                 return new ResponseDto<IEnumerable<ProductDto>>(true, "Similar ads retrieved", result);

            }
            catch (Exception ex)
            {
                 return new ResponseDto<IEnumerable<ProductDto>>(false, ex.Message, null!);
            }
        }
    }
}
