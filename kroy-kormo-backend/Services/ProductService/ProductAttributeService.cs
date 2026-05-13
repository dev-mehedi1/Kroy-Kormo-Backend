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

        public async Task<ResponseDto<IEnumerable<ProductAttributeDto>>> GetAttributesByCategorySlugAsync(string slug)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Slug == slug);
            if (category == null)
                return new ResponseDto<IEnumerable<ProductAttributeDto>>(false, "Category not found", null!);

            return await GetAttributesByCategoryAsync(category.CategoryId);
        }

        public async Task<ResponseDto<ProductAttributeDto>> CreateAttributeAsync(ProductAttributeDto attributeDto)
        {
            try
            {
                var attribute = _mapper.Map<ProductAttribute>(attributeDto);
                attribute.AttributeId = Guid.NewGuid();
                attribute.CreatedAt = DateTime.UtcNow;
                attribute.IsActive = true;

                await _context.ProductAttributes.AddAsync(attribute);
                await _context.SaveChangesAsync();

                var resultDto = _mapper.Map<ProductAttributeDto>(attribute);
                return new ResponseDto<ProductAttributeDto>(true, "Attribute created successfully", resultDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<ProductAttributeDto>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<ProductAttributeDto>> UpdateAttributeAsync(Guid attributeId, ProductAttributeDto attributeDto)
        {
            try
            {
                var attribute = await _context.ProductAttributes.FindAsync(attributeId);
                if (attribute == null)
                    return new ResponseDto<ProductAttributeDto>(false, "Attribute not found", null!);

                _mapper.Map(attributeDto, attribute);
                attribute.AttributeId = attributeId; // Ensure ID doesn't change
                attribute.UpdatedAt = DateTime.UtcNow;

                _context.ProductAttributes.Update(attribute);
                await _context.SaveChangesAsync();

                var resultDto = _mapper.Map<ProductAttributeDto>(attribute);
                return new ResponseDto<ProductAttributeDto>(true, "Attribute updated successfully", resultDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<ProductAttributeDto>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<bool>> DeleteAttributeAsync(Guid attributeId)
        {
            try
            {
                var attribute = await _context.ProductAttributes.FindAsync(attributeId);
                if (attribute == null)
                    return new ResponseDto<bool>(false, "Attribute not found", false);

                // Soft delete
                attribute.IsActive = false;
                attribute.UpdatedAt = DateTime.UtcNow;

                _context.ProductAttributes.Update(attribute);
                await _context.SaveChangesAsync();

                return new ResponseDto<bool>(true, "Attribute deleted (deactivated) successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseDto<bool>(false, ex.Message, false);
            }
        }
    }
}
