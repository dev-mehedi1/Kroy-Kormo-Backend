using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Category;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface;
using Sob_Need_Server.Models.Categories;

namespace Sob_Need_Server.Services.CategoryService
{
    public class CategoryService(AppDbContext context, IMapper mapper) : ICategoryService
         
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        // Creaet category
        public async Task<ResponseDto<CategoryDto>> CreateAsync(CategoryDto entity)
        {
            try
            {
                // Check if category name already exists
                bool exists = await _context.Categories.AnyAsync(x => x.CategoryName == entity.CategoryName);
                if (exists)
                {
                    return new ResponseDto<CategoryDto>(false, "Category name already exists.", null!);
                }

                // Map DTO to Entity
                var categoryEntity = _mapper.Map<Category>(entity);

                // Add to the database and save changes
                await _context.Categories.AddAsync(categoryEntity);
                await _context.SaveChangesAsync();

                var categoryDto = _mapper.Map<CategoryDto>(categoryEntity);

                return new ResponseDto<CategoryDto>(true, "Category created successfully.", categoryDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<CategoryDto>(false, $"An error occurred: {ex.Message}", null!);
            }
        }

        public Task<ResponseDto<CategoryDto>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<IEnumerable<CategoryDto>>> GetAllAsync(CategoryFilterDto filter)
        {
            try
            {
                // Start with the full set of categories
                var query = _context.Categories.AsQueryable();

                // Apply the search text filter (if any)
                if (!string.IsNullOrEmpty(filter.SearchText))
                {
                    query = query.Where(x => x.CategoryName.Contains(filter.SearchText));
                }

                // Apply the parentId filter (if any)
                if (filter.ParentCategoryId.HasValue)
                {
                    query = query.Where(x => x.ParentCategoryId == filter.ParentCategoryId.Value);
                }

                // Apply sorting based on the OrderByAsc flag
                if (filter.OrderByAsc.HasValue)
                {
                    query = filter.OrderByAsc.Value
                        ? query.OrderBy(x => x.CategoryName) // Ascending order
                        : query.OrderByDescending(x => x.CategoryName); // Descending order
                }

                // Apply pagination
                if (filter.Page.HasValue && filter.PageSize.HasValue)
                {
                    query = query.Skip((filter.Page.Value - 1) * filter.PageSize.Value)
                                 .Take(filter.PageSize.Value);
                }

                // Execute the query and map to CategoryDto
                var categories = await query
                    .Select(x => _mapper.Map<CategoryDto>(x))
                    .ToListAsync();

                // Total count for pagination
                var totalCount = await query.CountAsync();

                // Prepare the response
                var response = new ResponseDto<IEnumerable<CategoryDto>>(
                    true,
                    "Categories fetched successfully",
                    categories)
                {
                    TotalCount = totalCount
                };

                return response;
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return new ResponseDto<IEnumerable<CategoryDto>>(false, $"Error: {ex.Message}", null!);
            }
        }

        public Task<ResponseDto<CategoryDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<CategoryDto>> UpdateAsync(int id, CategoryDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
