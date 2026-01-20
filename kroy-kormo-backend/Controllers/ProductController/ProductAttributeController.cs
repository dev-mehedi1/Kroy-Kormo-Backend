using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.Interface.Products;
using Sob_Need_Server.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<ResponseDto<ProductAttributeDto>>> CreateAttribute([FromBody] ProductAttributeDto attributeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseDto<ProductAttributeDto>(false, "Invalid Data", null!));

            var result = await _productAttributeService.CreateAttributeAsync(attributeDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<ResponseDto<ProductAttributeDto>>> UpdateAttribute(Guid id, [FromBody] ProductAttributeDto attributeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseDto<ProductAttributeDto>(false, "Invalid Data", null!));

            var result = await _productAttributeService.UpdateAttributeAsync(id, attributeDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteAttribute(Guid id)
        {
            var result = await _productAttributeService.DeleteAttributeAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
