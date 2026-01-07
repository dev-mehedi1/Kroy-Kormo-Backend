using Microsoft.AspNetCore.SignalR;
using Sob_Need_Server.Interface;
using Sob_Need_Server.Interface.Authentication;
using Sob_Need_Server.Interface.Location;
using Sob_Need_Server.Interface.Notification;
using Sob_Need_Server.Interface.User;
using Sob_Need_Server.Interface.Chat;
using Sob_Need_Server.Interface.Analytics;
using Sob_Need_Server.Interface.Products;
using Sob_Need_Server.Services.Authentication;
using Sob_Need_Server.Services.CategoryService;
using Sob_Need_Server.Services.LocationService;
using Sob_Need_Server.Services.Notifications;
using Sob_Need_Server.Services.Notifications;
using Sob_Need_Server.Services.ProductService;
using Sob_Need_Server.Services.User;
using Sob_Need_Server.Services.Admin; // Added
using Sob_Need_Server.Interface.Admin; // Added
using Sob_Need_Server.Services.Chat;
using Sob_Need_Server.Services.Analytics;
using Sob_Need_Server.Services.ConditionService;
using Sob_Need_Server.SignalR;
using Sob_Need_Server.Utils.Helpers;
using Services.ProductService;
using Sob_Need_Server.Services.Jobs;

namespace Sob_Need_Server.Extensions;


public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application-level services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IConditionService, ConditionService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<TokenHelper>();
        services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IStateServices, StateService>();
        services.AddScoped<ICityServices, CityService>();
        services.AddScoped<IRegionServices, RegionService>();
        
        // Register user feature services
        services.AddScoped<IFavoriteService, FavoriteService>();
        services.AddScoped<ISavedSearchService, SavedSearchService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddScoped<IPromotionService, PromotionService>();
        services.AddScoped<IProductAttributeService, ProductAttributeService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IRankingService, RankingService>();
        services.AddScoped<ISuggestionService, SuggestionService>();
        
        // Job Listing Platform Services
        services.AddScoped<IJobService, JobService>();
        services.AddScoped<IJobApplicationService, JobApplicationService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<IRecruitmentService, RecruitmentService>();

        return services;

    }
}

