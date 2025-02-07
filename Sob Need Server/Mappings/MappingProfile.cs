using AutoMapper;
using Sob_Need_Server.DTOs.Attachment;
using Sob_Need_Server.DTOs.Authentication;
using Sob_Need_Server.DTOs.Category;
using Sob_Need_Server.DTOs.Location;
using Sob_Need_Server.DTOs.Notification;
using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.Models.Attachment;
using Sob_Need_Server.Models.Authentication;
using Sob_Need_Server.Models.Categories;
using Sob_Need_Server.Models.Locations;
using Sob_Need_Server.Models.Notification;
using Sob_Need_Server.Models.Products;

namespace Sob_Need_Server.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<Attachment, ImageDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<State, StateDto>().ReverseMap();
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<Region, RegionDto>().ReverseMap();
        }
    }
}
