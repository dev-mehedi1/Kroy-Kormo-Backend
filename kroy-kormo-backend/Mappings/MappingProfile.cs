using AutoMapper;
using Sob_Need_Server.DTOs.Attachment;
using Sob_Need_Server.DTOs.Authentication;
using Sob_Need_Server.DTOs.Category;
using Sob_Need_Server.DTOs.Location;
using Sob_Need_Server.DTOs.Notification;
using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.DTOs.User;
using Sob_Need_Server.Models.Authentication;
using Sob_Need_Server.Models.Categories;
using Sob_Need_Server.Models.Locations;
using Sob_Need_Server.Models.Notifications;
using Sob_Need_Server.Models.Products;
using Sob_Need_Server.Models.Users;
using Sob_Need_Server.Models.Chat;
using Sob_Need_Server.DTOs.Chat;
using Models.Products;
using AttachmentEntity = Models.Attachment.Attachment;
using DTOs.Product;

namespace Sob_Need_Server.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product mappings
            CreateMap<Product, ProductDto>()
                 .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Attachments))
                .ForMember(dest => dest.ConditionName, opt => opt.MapFrom(src => src.Condition != null ? src.Condition.ConditionName : null))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status != null ? src.Status.StatusName : null))
                .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : null))
                .ForMember(dest => dest.SellerPhone, opt => opt.MapFrom(src => src.User != null ? src.User.Phone : null))
                .ForMember(dest => dest.IsVerifiedSeller, opt => opt.MapFrom(src => src.User != null && src.User.IsVerified))
                .ForMember(dest => dest.UserTypeName, opt => opt.MapFrom(src => src.User != null && src.User.UserType != null ? src.User.UserType.TypeName : null))
                .ForMember(dest => dest.ActivePromotions, opt => opt.MapFrom(src => 
                    src.Promotions.Where(p => p.IsActive && p.EndDate >= DateTime.UtcNow)))
                .ForMember(dest => dest.Locations, opt => opt.MapFrom(src => src.Locations))
                .ForMember(dest => dest.StatusHistory, opt => opt.MapFrom(src => src.StatusHistories.OrderBy(sh => sh.CreatedAt)))
                .ForMember(dest => dest.CurrentStatusHistory, opt => opt.MapFrom(src => 
                    src.StatusHistories.OrderByDescending(sh => sh.CreatedAt).FirstOrDefault()))
                .ReverseMap();
            
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Locations, opt => opt.Ignore()); // Locations handled manually in Service
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Locations, opt => opt.Ignore()) // Locations handled manually in Service
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
            
            // ProductStatusHistory mapping
            CreateMap<ProductStatusHistory, ProductStatusHistoryDto>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status != null ? src.Status.StatusName : null))
                .ReverseMap();
            
            // Category mappings
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count))
                .ReverseMap();

            // Location mappings
            // Location mappings
            CreateMap<Location, LocationDto>()
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country != null ? src.Country.CountryName : null))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City != null ? src.City.CityName : null))
                .ForMember(dest => dest.RegionName, opt => opt.MapFrom(src => src.Region != null ? src.Region.RegionName : null))
                .ForMember(dest => dest.StateName, opt => opt.MapFrom(src => src.State != null ? src.State.StateName : null));

            // Attachment mappings
            //CreateMap<AttachmentEntity, ImageDto>().ReverseMap();

            CreateMap<AttachmentEntity, ImageDto>()
            .ForMember(dest => dest.AttachmentId, opt => opt.MapFrom(src => src.AttachmentId))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

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

            // Promotion mappings
            CreateMap<ProductPromotion, PromotionDto>().ReverseMap();

            // Chat mappings
            CreateMap<Conversation, ConversationDto>()
                .ForMember(dest => dest.OtherUserId, opt => opt.MapFrom((src, dest, _, context) => 
                {
                    var userId = (Guid)context.Items["UserId"];
                    return src.User1Id == userId ? src.User2Id : src.User1Id;
                }))
                .ForMember(dest => dest.OtherUserName, opt => opt.MapFrom((src, dest, _, context) => 
                {
                    var userId = (Guid)context.Items["UserId"];
                    return src.User1Id == userId ? src.User2.FullName : src.User1.FullName;
                }))
                .ForMember(dest => dest.OtherUserImageUrl, opt => opt.MapFrom((src, dest, _, context) => 
                {
                    var userId = (Guid)context.Items["UserId"];
                    return src.User1Id == userId ? src.User2.ProfilePicture : src.User1.ProfilePicture;
                }))
                .ForMember(dest => dest.UnreadCount, opt => opt.MapFrom((src, dest, _, context) => 
                {
                    var userId = (Guid)context.Items["UserId"];
                    return src.Messages.Count(m => m.SenderId != userId && !m.IsRead);
                }));

            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.FullName))
                .ForMember(dest => dest.SenderImageUrl, opt => opt.MapFrom(src => src.Sender.ProfilePicture));
            
            // Country/State/City/Region mappings
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<State, StateDto>().ReverseMap();
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<Region, RegionDto>().ReverseMap();
        }
    }
}

