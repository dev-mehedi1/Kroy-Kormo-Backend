using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Analytics;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface.Analytics;
using Sob_Need_Server.Models.Products;

namespace Sob_Need_Server.Services.Analytics
{
    public class AnalyticsService(AppDbContext context) : IAnalyticsService
    {
        private readonly AppDbContext _context = context;

        public async Task<ResponseDto<SellerStatsDto>> GetSellerStatsAsync(Guid userId)
        {
            try
            {
                var userAds = await _context.Products
                    .Where(p => p.UserId == userId)
                    .ToListAsync();

                var productIds = userAds.Select(p => p.ProductId).ToList();

                var stats = new SellerStatsDto
                {
                    TotalActiveAds = userAds.Count(p => p.IsActive),
                    TotalViews = await _context.ProductViews.CountAsync(pv => productIds.Contains(pv.ProductId)),
                    TotalFavorites = await _context.Favorites.CountAsync(f => productIds.Contains(f.ProductId)),
                    TotalConversations = await _context.Conversations.CountAsync(c => c.User1Id == userId || c.User2Id == userId),
                };

                // Get Daily Stats for the last 14 days
                var fourteenDaysAgo = DateTime.UtcNow.Date.AddDays(-14);
                
                var dailyViews = await _context.ProductViews
                    .Where(pv => productIds.Contains(pv.ProductId) && pv.ViewedAt >= fourteenDaysAgo)
                    .GroupBy(pv => pv.ViewedAt.Date)
                    .Select(g => new { Date = g.Key, Count = g.Count() })
                    .ToListAsync();

                var dailyFavorites = await _context.Favorites
                    .Where(f => productIds.Contains(f.ProductId) && f.CreatedAt >= fourteenDaysAgo)
                    .GroupBy(f => f.CreatedAt.Date)
                    .Select(g => new { Date = g.Key, Count = g.Count() })
                    .ToListAsync();

                var allDates = Enumerable.Range(0, 15)
                    .Select(i => fourteenDaysAgo.AddDays(i))
                    .ToList();

                stats.DailyStats = allDates.Select(date => new DailyStatDto
                {
                    Date = date,
                    Views = dailyViews.FirstOrDefault(v => v.Date == date)?.Count ?? 0,
                    Favorites = dailyFavorites.FirstOrDefault(f => f.Date == date)?.Count ?? 0
                }).ToList();

                // Get Top 5 Products
                stats.TopProducts = await _context.Products
                    .Where(p => p.UserId == userId)
                    .OrderByDescending(p => _context.ProductViews.Count(pv => pv.ProductId == p.ProductId))
                    .Take(5)
                    .Select(p => new TopProductStatDto
                    {
                        ProductId = p.ProductId,
                        Title = p.Title,
                        Views = _context.ProductViews.Count(pv => pv.ProductId == p.ProductId),
                        Favorites = _context.Favorites.Count(f => f.ProductId == p.ProductId),
                        ImageUrl = _context.Attachments.Where(a => a.ProductId == p.ProductId).Select(a => a.ImageUrl).FirstOrDefault()
                    })
                    .ToListAsync();

                return new ResponseDto<SellerStatsDto>(true, "Analytics retrieved successfully", stats);
            }
            catch (Exception ex)
            {
                return new ResponseDto<SellerStatsDto>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<bool>> TrackProductViewAsync(Guid productId, Guid? userId, string ipAddress, string userAgent)
        {
            try
            {
                // Prevent over-counting: Only one view per IP/Product every 24 hours
                var last24Hours = DateTime.UtcNow.AddHours(-24);
                var recentView = await _context.ProductViews
                    .AnyAsync(pv => pv.ProductId == productId && 
                                   pv.IpAddress == ipAddress && 
                                   pv.ViewedAt >= last24Hours);

                if (recentView) return new ResponseDto<bool>(true, "View already logged", true);

                var view = new ProductView
                {
                    ProductId = productId,
                    UserId = userId,
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    ViewedAt = DateTime.UtcNow
                };

                _context.ProductViews.Add(view);
                
                // Increment view count directly on product for performance discovery
                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    product.ViewCount++;
                }

                await _context.SaveChangesAsync();
                return new ResponseDto<bool>(true, "View tracked", true);
            }
            catch (Exception ex)
            {
                return new ResponseDto<bool>(false, ex.Message, false);
            }
        }
        public async Task<ResponseDto<DashboardStatsDto>> GetDashboardStatsAsync()
        {
            try
            {
                var stats = new DashboardStatsDto
                {
                    TotalUsers = await _context.Users.CountAsync(),
                    ActiveUsers = await _context.Users.CountAsync(u => u.IsActive),
                    TotalListings = await _context.Products.CountAsync(),
                    ActiveListings = await _context.Products.CountAsync(p => p.IsActive),
                    TotalRevenue = 0 // Placeholder
                };

                // User Growth (Last 7 days)
                var sevenDaysAgo = DateTime.UtcNow.Date.AddDays(-7);
                var userGrowth = await _context.Users
                    .Where(u => u.CreatedAt >= sevenDaysAgo)
                    .GroupBy(u => u.CreatedAt.Date)
                    .Select(g => new { Date = g.Key, Count = g.Count() })
                    .ToListAsync();
                
                var allDates = Enumerable.Range(0, 8)
                    .Select(i => sevenDaysAgo.AddDays(i))
                    .ToList();

                stats.UserGrowth = allDates.Select(date => new DailyStatDto
                {
                    Date = date,
                    Views = userGrowth.FirstOrDefault(u => u.Date == date)?.Count ?? 0, // Reuse Views field for Count to simplify DTO usage or create new field
                    Favorites = 0
                }).ToList();

                // Category Distribution
                stats.CategoryDistribution = await _context.Categories
                    .Select(c => new CategoryStatDto
                    {
                        CategoryName = c.CategoryName,
                        Count = c.Products.Count
                    })
                    .OrderByDescending(c => c.Count)
                    .Take(5)
                    .ToListAsync();

                return new ResponseDto<DashboardStatsDto>(true, "Dashboard stats retrieved", stats);
            }
            catch (Exception ex)
            {
                return new ResponseDto<DashboardStatsDto>(false, ex.Message, null!);
            }
        }
    }
}
