using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.Interface.Products;
using Sob_Need_Server.Models.Products;

namespace Sob_Need_Server.Services.ProductService
{
    public class ProductAttributeService(AppDbContext context, IMapper mapper) : IProductAttributeService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseDto<IEnumerable<ProductAttributeDto>>> GetAllAttributesAsync()
        {
            try
            {
                var attributes = await _context.ProductAttributes
                    .Where(a => a.IsActive)
                    .OrderBy(a => a.DisplayOrder)
                    .ToListAsync();

                var attributeDtos = _mapper.Map<IEnumerable<ProductAttributeDto>>(attributes);
                return new ResponseDto<IEnumerable<ProductAttributeDto>>(true, "Attributes retrieved successfully", attributeDtos);
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<ProductAttributeDto>>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<ProductAttributeDto>> GetAttributeByIdAsync(Guid attributeId)
        {
            try
            {
                var attribute = await _context.ProductAttributes
                    .FirstOrDefaultAsync(a => a.AttributeId == attributeId);

                if (attribute == null)
                    return new ResponseDto<ProductAttributeDto>(false, "Attribute not found", null!);

                var attributeDto = _mapper.Map<ProductAttributeDto>(attribute);
                return new ResponseDto<ProductAttributeDto>(true, "Attribute retrieved successfully", attributeDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<ProductAttributeDto>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<IEnumerable<ProductAttributeDto>>> GetAttributesByCategoryAsync(Guid categoryId)
        {
            try
            {
                var categoryIds = new List<Guid>();
                var currentId = (Guid?)categoryId;

                while (currentId.HasValue)
                {
                    categoryIds.Add(currentId.Value);
                    
                    var category = await _context.Categories
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.CategoryId == currentId);
                    
                    currentId = category?.ParentCategoryId;
                }

                var attributes = await _context.ProductAttributes
                    .Where(a => categoryIds.Contains(a.CategoryId) && a.IsActive)
                    .OrderBy(a => a.DisplayOrder)
                    .ToListAsync();

                var attributeDtos = _mapper.Map<IEnumerable<ProductAttributeDto>>(attributes);
                return new ResponseDto<IEnumerable<ProductAttributeDto>>(true, "Attributes retrieved successfully", attributeDtos);
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<ProductAttributeDto>>(false, ex.Message, null!);
            }
        }
    }
}
