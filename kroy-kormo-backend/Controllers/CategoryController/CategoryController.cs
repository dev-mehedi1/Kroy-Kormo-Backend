using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sob_Need_Server.DTOs.Category;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.Interface;
using Sob_Need_Server.Utils;
using DTOs.Category;

namespace Sob_Need_Server.Controllers.CategoryController
{
    [Route(Constants.BasePath + "/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        private readonly ICategoryService _categoryService = categoryService;

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin,Owner")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto<CategoryDto>(false, "Invalid request data.", null!));
            }

            var response = await _categoryService.CreateAsync(categoryDto);

            if (response.IsSuccess)
            {
                return CreatedAtAction(nameof(CreateCategory), new { id = response.Data?.CategoryId }, response);
            }

            return BadRequest(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetCategory([FromQuery] CategoryFilterDto filter)
        {
            var response = await _categoryService.GetAllAsync(filter);
            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
