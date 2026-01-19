using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.Interface.Products;
using Sob_Need_Server.Utils;

namespace Sob_Need_Server.Controllers.ProductController
{
    [ApiController]
    [Route(Constants.BasePath + "/[controller]")]
    public class ProductAttributeController(IProductAttributeService productAttributeService) : ControllerBase
    {
        private readonly IProductAttributeService _productAttributeService = productAttributeService;

        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<ProductAttributeDto>>>> GetAllAttributes()
        {
            var result = await _productAttributeService.GetAllAttributesAsync();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResponseDto<ProductAttributeDto>>> GetAttributeById(Guid id)
        {
            var result = await _productAttributeService.GetAttributeByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("category/{categoryIdOrSlug}")]
        public async Task<ActionResult<ResponseDto<IEnumerable<ProductAttributeDto>>>> GetAttributesByCategory(string categoryIdOrSlug)
        {
            if (Guid.TryParse(categoryIdOrSlug, out Guid categoryId))
            {
                var result = await _productAttributeService.GetAttributesByCategoryAsync(categoryId);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            else
            {
                var result = await _productAttributeService.GetAttributesByCategorySlugAsync(categoryIdOrSlug);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
        }
    }
}
