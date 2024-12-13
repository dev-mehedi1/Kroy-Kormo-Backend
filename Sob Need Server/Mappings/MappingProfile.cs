using AutoMapper;
using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.Models.Products;

namespace Sob_Need_Server.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
