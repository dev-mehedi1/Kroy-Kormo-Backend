using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public Task<ResponseDto<bool>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
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
