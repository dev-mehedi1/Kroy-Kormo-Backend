using AutoMapper;
using Sob_Need_Server.DTOs.Attachment;
using Sob_Need_Server.DTOs.Authentication;
using Sob_Need_Server.DTOs.Category;
using Sob_Need_Server.DTOs.Location;
using Sob_Need_Server.DTOs.Notification;
using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.DTOs.User;
using Sob_Need_Server.Models.Attachment;
using Sob_Need_Server.Models.Authentication;
using Sob_Need_Server.Models.Categories;
using Sob_Need_Server.Models.Locations;
using Sob_Need_Server.Models.Notification;
using Sob_Need_Server.Models.Products;
using Sob_Need_Server.Models.Users;

namespace Sob_Need_Server.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product mappings
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ConditionName, opt => opt.MapFrom(src => src.Condition != null ? src.Condition.ConditionName : null))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status != null ? src.Status.StatusName : null))
                .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : null))
                .ForMember(dest => dest.SellerPhone, opt => opt.MapFrom(src => src.User != null ? src.User.Phone : null))
                .ForMember(dest => dest.IsVerifiedSeller, opt => opt.MapFrom(src => src.User != null && src.User.IsVerified))
                .ForMember(dest => dest.UserTypeName, opt => opt.MapFrom(src => src.User != null && src.User.UserType != null ? src.User.UserType.TypeName : null))
                .ReverseMap();
            
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            
            // Condition mappings
            CreateMap<Condition, ConditionDto>().ReverseMap();
            
            // ProductStatus mappings
            CreateMap<ProductStatus, ProductStatusDto>().ReverseMap();
            
            // ProductAttribute mappings
            CreateMap<ProductAttribute, ProductAttributeDto>().ReverseMap();
            CreateMap<ProductAttributeValue, ProductAttributeValueDto>()
                .ForMember(dest => dest.AttributeName, opt => opt.MapFrom(src => src.Attribute != null ? src.Attribute.AttributeName : null))
                .ReverseMap();
            
            // Category mappings
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count))
                .ReverseMap();
            
            // Location mappings
            CreateMap<Location, LocationDto>().ReverseMap();
            
            // Attachment mappings
            CreateMap<Attachment, ImageDto>().ReverseMap();
            
            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserTypeName, opt => opt.MapFrom(src => src.UserType != null ? src.UserType.TypeName : null))
                .ReverseMap()
                .ForMember(dest => dest.Password, opt => opt.Ignore()); // Don't map password from DTO
            
            CreateMap<UserRegistrationDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore()) // Password is hashed separately
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            
            CreateMap<UpdateUserDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            
            CreateMap<User, UserProfileDto>()
                .ForMember(dest => dest.UserTypeName, opt => opt.MapFrom(src => src.UserType != null ? src.UserType.TypeName : null))
                .ForMember(dest => dest.TotalAds, opt => opt.MapFrom(src => src.Products.Count))
                .ForMember(dest => dest.ActiveAds, opt => opt.MapFrom(src => src.Products.Count(p => p.IsActive)))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => 
                    src.ReviewsReceived.Any() ? src.ReviewsReceived.Average(r => r.Rating) : (double?)null))
                .ForMember(dest => dest.TotalReviews, opt => opt.MapFrom(src => src.ReviewsReceived.Count));
            
            // UserType mappings
            CreateMap<UserType, UserDto>().ReverseMap();
            
            // Favorite mappings
            CreateMap<Favorite, FavoriteDto>().ReverseMap();
            
            // SavedSearch mappings
            CreateMap<SavedSearch, SavedSearchDto>().ReverseMap();
            
            // Review mappings
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.Seller != null ? src.Seller.FullName : null))
                .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => src.Reviewer != null ? src.Reviewer.FullName : null))
                .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product != null ? src.Product.Title : null))
                .ReverseMap();
            
            // Notification mappings
            CreateMap<Notification, NotificationDto>().ReverseMap();
            
            // Country/State/City/Region mappings
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<State, StateDto>().ReverseMap();
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<Region, RegionDto>().ReverseMap();
        }
    }
}

