using AutoMapper;
using AutoMapper.QueryableExtensions;
using DTOs.Category;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Category;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface;
using Sob_Need_Server.Models.Categories;
using Sob_Need_Server.Utils.Helpers;

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

                // Generate slug from the category name
                string baseSlug = StringUtilities.GenerateSlug(entity.CategoryName);
                string slug = baseSlug;
                int count = 1;

                while (await _context.Categories.AnyAsync(x => x.Slug == slug))
                {
                    slug = $"{baseSlug}-{count}";
                    count++;
                }

                // Map DTO to Entity
                var categoryEntity = _mapper.Map<Category>(entity);

                // Assign the generated slug to the entity
                categoryEntity.Slug = slug;

                // Add to the database and save changes
                await _context.Categories.AddAsync(categoryEntity);
                await _context.SaveChangesAsync();

                // Map back to DTO for the response
                var categoryDto = _mapper.Map<CategoryDto>(categoryEntity);

                return new ResponseDto<CategoryDto>(true, "Category created successfully.", categoryDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<CategoryDto>(false, $"An error occurred: {ex.Message}", null!);
            }
        }


        public Task<ResponseDto<CategoryDto>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<IEnumerable<CategoryDto>>> GetAllAsyncWithOutTree(CategoryFilterDto filter)
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

                // Apply the Type filter (if any)
                if (filter.Type.HasValue)
                {
                    query = query.Where(x => (int)x.Type == filter.Type.Value);
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
                //var categories = await query
                //    .Select(x => _mapper.Map<CategoryDto>(x))
                //    .ToListAsync();

                var categories = await query
                   .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
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


        public async Task<ResponseDto<IEnumerable<CategoryDto>>> GetAllAsync(CategoryFilterDto filter)
        {
            try
            {
                // Get all categories that match search criteria
                var query = _context.Categories.AsQueryable();

                if (!string.IsNullOrEmpty(filter.SearchText))
                {
                    query = query.Where(x => x.CategoryName.Contains(filter.SearchText));
                }

                // Apply the Type filter (if any)
                if (filter.Type.HasValue)
                {
                    query = query.Where(x => (int)x.Type == filter.Type.Value);
                }

                // Get all categories as flat list
                var allCategories = await query
                    .Select(x => new CategoryDto
                    {
                        CategoryId = x.CategoryId,
                        CategoryName = x.CategoryName,
                        Slug = x.Slug,
                        ParentCategoryId = x.ParentCategoryId,
                        Type = x.Type.ToString(),
                        ProductCount = x.Products.Count
                    })
                    .ToListAsync();

                // Build hierarchical tree
                var categoryTree = BuildCategoryTree(allCategories);

                // Apply pagination to root categories only if needed
                if (filter.Page.HasValue && filter.PageSize.HasValue)
                {
                    var rootCategories = categoryTree.Where(c => c.ParentCategoryId == null).ToList();
                    var paginatedRoots = rootCategories
                        .Skip((filter.Page.Value - 1) * filter.PageSize.Value)
                        .Take(filter.PageSize.Value)
                        .ToList();

                    // Get complete tree for the paginated roots
                    var result = categoryTree
                        .Where(c => paginatedRoots.Any(rc => rc.CategoryId == c.CategoryId))
                        .ToList();

                    var response = new ResponseDto<IEnumerable<CategoryDto>>(
                        true,
                        "Categories fetched successfully",
                        result)
                    {
                        TotalCount = rootCategories.Count
                    };
                    return response;
                }

                // Return only root categories if no pagination
                var rootCategoryTree = categoryTree.Where(c => c.ParentCategoryId == null).ToList();

                return new ResponseDto<IEnumerable<CategoryDto>>(
                    true,
                    "Categories fetched successfully",
                    rootCategoryTree)
                {
                    TotalCount = rootCategoryTree.Count
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<CategoryDto>>(false, $"Error: {ex.Message}", null!);
            }
        }

        private static List<CategoryDto> BuildCategoryTree(List<CategoryDto> allCategories)
        {
            // Dictionary for quick lookup
            var categoryDict = allCategories.ToDictionary(c => c.CategoryId);
            var rootCategories = new List<CategoryDto>();

            foreach (var category in allCategories)
            {
                if (category.ParentCategoryId.HasValue)
                {
                    // Find parent and add this category as child
                    if (categoryDict.TryGetValue(category.ParentCategoryId.Value, out var parentCategory))
                    {
                        parentCategory.Children.Add(category);
                    }
                }
                else
                {
                    // This is a root category
                    rootCategories.Add(category);
                }
            }

            return rootCategories;
        }

        public Task<ResponseDto<CategoryDto>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<CategoryDto>> UpdateAsync(Guid id, CategoryDto entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<CategoryDto>> GetBySlugAsync(string slug)
        {
            try
            {
                var category = await _context.Categories
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.Slug == slug);

                if (category == null)
                {
                    return new ResponseDto<CategoryDto>(false, "Category not found", null!);
                }

                var categoryDto = _mapper.Map<CategoryDto>(category);
                
                // If it's a parent category, we might want to fill children? 
                // Currently GetAllAsync does full tree building, but GetBySlug usually views one category.
                // Keeping it simple mapping for now.

                return new ResponseDto<CategoryDto>(true, "Category retrieved successfully", categoryDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<CategoryDto>(false, ex.Message, null!);
            }
        }
    }
}
