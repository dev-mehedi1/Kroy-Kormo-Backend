namespace Sob_Need_Server.DTOs.Product;

public class ProductFilterDto
{
    // Pagination
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 20;
    
    // Search
    public string? SearchText { get; set; }
    
    // Category filters
    public string? CategorySlug { get; set; }
    public Guid? CategoryId { get; set; }
    
    // Location filters
    public Guid? CountryId { get; set; }
    public Guid? CityId { get; set; }
    public Guid? RegionId { get; set; }
    public Guid? StateId { get; set; }
    
    // Price filters
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    
    // Condition filter
    public Guid? ConditionId { get; set; }
    
    // Status filter
    public Guid? StatusId { get; set; }
    
    // User/Poster filters
    public Guid? UserId { get; set; }
    public Guid? UserTypeId { get; set; } // Filter by poster type (Individual, Shop, Business)
    public bool? IsVerifiedSeller { get; set; }
    
    // Product type filters
    public bool? IsTopAd { get; set; }
    public bool? IsUrgentAd { get; set; }
    public bool? IsFeaturedMember { get; set; }
    public bool? IsJobAd { get; set; }
    public bool? IsSell { get; set; }
    public bool? IsDeliveryFree { get; set; }
    
    // Date filters
    public DateTime? PostedAfter { get; set; }
    public DateTime? PostedBefore { get; set; }
    
    // Sorting
    public string? SortBy { get; set; } // "newest", "price_low", "price_high", "popular"
    public bool? OrderByAsc { get; set; }
    
    // Include only active products by default
    public bool? IsActive { get; set; } = true;
    public bool? IsManagementMode { get; set; }
}

