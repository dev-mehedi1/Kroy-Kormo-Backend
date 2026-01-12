using AutoMapper;
using Sob_Need_Server.DTOs.Attachment;
using Sob_Need_Server.DTOs.Authentication;
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
using Models.Locations;
using DTOs.Category;
using Sob_Need_Server.DTOs.Jobs;

namespace Mappings
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
                .ForMember(dest => dest.StatusSlug, opt => opt.MapFrom(src => src.Status != null ? src.Status.Slug : null))
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
                // Add these category mappings:
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryId : (Guid?)null))
                .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => src.Condition != null ? src.Condition.ConditionName : null))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Locations.FirstOrDefault() != null ? src.Locations.FirstOrDefault()!.AddressLine : null))
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.Ignore()) // Add this to reverse map
                .ReverseMap();
            
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Locations, opt => opt.Ignore()); // Locations handled manually in Service
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Locations, opt => opt.Ignore()) // Locations handled manually in Service
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            
            // Condition mappings
            CreateMap<Condition, ConditionDto>().ReverseMap();
            
            // ProductStatus mappings
            CreateMap<ProductStatus, ProductStatusDto>()
                .ForMember(dest => dest.StatusSlug, opt => opt.MapFrom(src => src.Slug))
                .ReverseMap();
            
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
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count))
                .ReverseMap();

            // Location mappings
            CreateMap<Location, LocationDto>()
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src =>
                    src.Country != null ? src.Country.CountryName : null))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src =>
                    src.City != null ? src.City.CityName : null))
                .ForMember(dest => dest.RegionName, opt => opt.MapFrom(src =>
                    src.Region != null ? src.Region.RegionName : null))
                .ForMember(dest => dest.StateName, opt => opt.MapFrom(src =>
                    src.State != null ? src.State.StateName : null))
                .ReverseMap();


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
                .ForMember(dest => dest.ParticipantId, opt => opt.MapFrom((src, dest, _, context) => 
                {
                    var userId = (Guid)context.Items["UserId"];
                    return src.User1Id == userId ? src.User2Id : src.User1Id;
                }))
                .ForMember(dest => dest.ParticipantName, opt => opt.MapFrom((src, dest, _, context) => 
                {
                    var userId = (Guid)context.Items["UserId"];
                    return src.User1Id == userId ? src.User2.FullName : src.User1.FullName;
                }))
                .ForMember(dest => dest.ParticipantImage, opt => opt.MapFrom((src, dest, _, context) => 
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
                .ForMember(dest => dest.SenderImageUrl, opt => opt.MapFrom(src => src.Sender.ProfilePicture))
                .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product != null ? src.Product.Title : null))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product != null && src.Product.Attachments.Any() ? src.Product.Attachments.FirstOrDefault()!.ImageUrl : null));
            
            // Country/State/City/Region mappings
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<State, StateDto>().ReverseMap();
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<Region, RegionDto>().ReverseMap();
            
            // Job-related mappings
            CreateMap<Sob_Need_Server.Models.Jobs.JobType, JobTypeDto>().ReverseMap();
            CreateMap<Sob_Need_Server.Models.Jobs.ExperienceLevel, ExperienceLevelDto>().ReverseMap();
            CreateMap<Sob_Need_Server.Models.Jobs.Skill, SkillDto>().ReverseMap();
            
            CreateMap<Sob_Need_Server.Models.Jobs.JobListing, JobListingDto>()

                .ForMember(dest => dest.EmployerName, opt => opt.MapFrom(src => src.Employer != null ? src.Employer.FullName : null))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyProfile != null ? src.CompanyProfile.CompanyName : null))
                .ForMember(dest => dest.CompanyLogoUrl, opt => opt.MapFrom(src => src.CompanyProfile != null ? src.CompanyProfile.LogoUrl : null))
                .ForMember(dest => dest.JobTypeName, opt => opt.MapFrom(src => src.JobType != null ? src.JobType.TypeName : null))
                .ForMember(dest => dest.ExperienceLevelName, opt => opt.MapFrom(src => src.ExperienceLevel != null ? src.ExperienceLevel.LevelName : null))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Product != null ? src.Product.Title : null))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Product != null ? src.Product.Description : null))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Product != null ? src.Product.Slug : null))
                .ForMember(dest => dest.RequiredSkills, opt => opt.MapFrom(src => src.RequiredSkills.Select(rs => rs.Skill)))
                .ReverseMap();

            CreateMap<CreateJobListingDto, Sob_Need_Server.Models.Jobs.JobListing>()
                .ForMember(dest => dest.RequiredSkills, opt => opt.Ignore());

            CreateMap<Sob_Need_Server.Models.Jobs.JobApplication, JobApplicationDto>()
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job != null && src.Job.Product != null ? src.Job.Product.Title : null))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Job != null && src.Job.CompanyProfile != null ? src.Job.CompanyProfile.CompanyName : null))
                .ForMember(dest => dest.ApplicantName, opt => opt.MapFrom(src => src.Applicant != null ? src.Applicant.FullName : null))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status != null ? src.Status.StatusName : null))
                .ForMember(dest => dest.StatusColorCode, opt => opt.MapFrom(src => src.Status != null ? src.Status.ColorCode : null))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Applicant != null ? src.Applicant.Email : null))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Applicant != null ? src.Applicant.Phone : null))
                .ForMember(dest => dest.Educations, opt => opt.MapFrom(src => src.Applicant != null ? src.Applicant.Educations : null))
                .ForMember(dest => dest.Experiences, opt => opt.MapFrom(src => src.Applicant != null ? src.Applicant.Experiences : null))
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Applicant != null ? src.Applicant.Skills : null))
                .ReverseMap();


            CreateMap<CreateApplicationDto, Sob_Need_Server.Models.Jobs.JobApplication>().ReverseMap();
            CreateMap<Sob_Need_Server.Models.Jobs.ApplicationStatusHistory, ApplicationStatusHistoryDto>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status != null ? src.Status.StatusName : null))
                .ReverseMap();

            // User Profile Extension mappings
            CreateMap<UserEducation, UserEducationDto>().ReverseMap();
            CreateMap<UserExperience, UserExperienceDto>().ReverseMap();
            CreateMap<UserSkill, UserSkillDto>()
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill != null ? src.Skill.SkillName : null))
                .ReverseMap();
            CreateMap<UserResume, UserResumeDto>().ReverseMap();
            
            CreateMap<User, ExtendedProfileDto>()
                .ForMember(dest => dest.Educations, opt => opt.MapFrom(src => src.Educations))
                .ForMember(dest => dest.Experiences, opt => opt.MapFrom(src => src.Experiences))
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills))
                .ForMember(dest => dest.Resumes, opt => opt.MapFrom(src => src.Resumes));

            // Recruitment mappings
            CreateMap<Sob_Need_Server.Models.Jobs.CompanyProfile, CompanyProfileDto>().ReverseMap();
            CreateMap<Sob_Need_Server.Models.Jobs.Interview, InterviewDto>()
                .ForMember(dest => dest.InterviewId, opt => opt.MapFrom(src => src.InterviewId))
                .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.ApplicationId))
                .ForMember(dest => dest.JobId, opt => opt.MapFrom(src => src.Application != null && src.Application.Job != null ? src.Application.Job.JobId : Guid.Empty))
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Application != null && src.Application.Job != null && src.Application.Job.Product != null ? src.Application.Job.Product.Title : null))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Application != null && src.Application.Job != null && src.Application.Job.CompanyProfile != null ? src.Application.Job.CompanyProfile.CompanyName : null))
                .ForMember(dest => dest.CompanyLogo, opt => opt.MapFrom(src => src.Application != null && src.Application.Job != null && src.Application.Job.CompanyProfile != null ? src.Application.Job.CompanyProfile.LogoUrl : null))
                .ForMember(dest => dest.CandidateName, opt => opt.MapFrom(src => src.Candidate != null ? src.Candidate.FullName : null))
                .ReverseMap();
            CreateMap<Sob_Need_Server.Models.Jobs.JobOffer, JobOfferDto>()
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobTitle))
                .ReverseMap();

            // Job Alert mappings
            CreateMap<Sob_Need_Server.Models.Jobs.JobAlert, JobAlertDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
                .ForMember(dest => dest.JobTypeName, opt => opt.MapFrom(src => src.JobType != null ? src.JobType.TypeName : null))
                .ForMember(dest => dest.ExperienceLevelName, opt => opt.MapFrom(src => src.ExperienceLevel != null ? src.ExperienceLevel.LevelName : null))
                .ReverseMap();
            CreateMap<CreateJobAlertDto, Sob_Need_Server.Models.Jobs.JobAlert>();
            CreateMap<UpdateJobAlertDto, Sob_Need_Server.Models.Jobs.JobAlert>();

            // Job View mappings
            CreateMap<Sob_Need_Server.Models.Jobs.JobView, JobViewDto>().ReverseMap();

            // Recommendation mappings
            CreateMap<Sob_Need_Server.Models.Jobs.JobListing, JobWithScoreDto>().IncludeBase<Sob_Need_Server.Models.Jobs.JobListing, JobListingDto>();
            CreateMap<Sob_Need_Server.Models.Jobs.JobListing, TrendingJobDto>().IncludeBase<Sob_Need_Server.Models.Jobs.JobListing, JobListingDto>();

            // Saved Job mappings
            CreateMap<Sob_Need_Server.Models.Jobs.SavedJob, SavedJobDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Job!.Product!.Title))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => 
                    src.Job!.CompanyProfile != null ? src.Job.CompanyProfile.CompanyName : 
                    (src.Job.Employer != null ? src.Job.Employer.FullName : 
                    (src.Job.Product != null && src.Job.Product.User != null ? src.Job.Product.User.FullName : "Unknown Company"))))
                .ForMember(dest => dest.CompanyLogoUrl, opt => opt.MapFrom(src => src.Job!.CompanyProfile != null ? src.Job.CompanyProfile.LogoUrl : null))
                .ForMember(dest => dest.JobTypeName, opt => opt.MapFrom(src => src.Job!.JobType!.TypeName))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Job!.OfficeLocation))
                .ForMember(dest => dest.IsRemote, opt => opt.MapFrom(src => src.Job!.IsRemote))
                .ForMember(dest => dest.SalaryMin, opt => opt.MapFrom(src => src.Job!.SalaryMin))
                .ForMember(dest => dest.SalaryMax, opt => opt.MapFrom(src => src.Job!.SalaryMax))
                .ForMember(dest => dest.SalaryCurrency, opt => opt.MapFrom(src => src.Job!.SalaryCurrency))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Job!.CreatedAt))
                .ForMember(dest => dest.RequiredSkills, opt => opt.MapFrom(src => src.Job!.RequiredSkills.Select(rs => rs.Skill)));
        }
    }
}


