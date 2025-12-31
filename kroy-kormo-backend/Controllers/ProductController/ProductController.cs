using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.Interface;
using Sob_Need_Server.Utils;
using Sob_Need_Server.Utils.Helpers;
using Sob_Need_Server.DTOs.Common;

namespace Sob_Need_Server.Controllers.ProductController
{
    [Route(Constants.BasePath + "/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

      /// <summary>
      /// Get All products with filters
      /// </summary>
      /// <param name="Filters"></param>
      /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<ProductDto>>>> GetAllProducts([FromQuery] ProductFilterDto Filters)
        {
            var response = await _productService.GetAllProductsAsync(Filters);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        // GET: api/product/5
        [HttpGet("byid/{id}")]
        public async Task<ActionResult<ResponseDto<ProductDto>>> GetProductById(Guid id)
        {
            var response = await _productService.GetByIdAsync(id);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        // POST: api/product
        [HttpPost]
        public async Task<ActionResult<ResponseDto<ProductDto>>> CreateProduct(CreateProductDto productDto)
        {
            var response = await _productService.CreateAsync(productDto);
            if (!response.IsSuccess)
                return BadRequest( response);

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = response.Data?.ProductId },
                response
            );
        }

        // PUT: api/product/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<ProductDto>>> UpdateProduct(Guid id, UpdateProductDto productDto)
        {
            var response = await _productService.UpdateAsync(id, productDto);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        // Get Product by using slug
        [HttpGet("{slug}")]
        public async Task<ActionResult<ResponseDto<ProductDto>>> GetProductBySlug(string slug)
        {
            var resonse = await _productService.GetProductBySlugAsync(slug);
            if (!resonse.IsSuccess) BadRequest(resonse);
            return Ok(resonse);
            
        }
        // DELETE: api/v1/product/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ProductDto>>> DeleteProduct(Guid id)
        {
            var userIdStr = JwtHelper.GetUserIdFromToken(Request.Headers.Authorization.ToString()!);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            var userId = Guid.Parse(userIdStr);

            var response = await _productService.DeleteAsync(id, userId);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        // PATCH: api/v1/product/5/status
        [HttpPatch("{id}/status")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ProductDto>>> UpdateProductStatus(Guid id, [FromBody] StatusUpdateDto updateDto)
        {
            var userIdStr = JwtHelper.GetUserIdFromToken(Request.Headers.Authorization.ToString()!);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            var userId = Guid.Parse(userIdStr);

            var response = await _productService.UpdateStatusAsync(id, updateDto.StatusSlug, userId);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
        // PATCH: api/v1/product/5/status-by-id
        [HttpPatch("{id}/status-by-id")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ProductDto>>> UpdateProductStatusById(Guid id, [FromBody] StatusUpdateByIdDto updateDto)
        {
            var userIdStr = JwtHelper.GetUserIdFromToken(Request.Headers.Authorization.ToString()!);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            var userId = Guid.Parse(userIdStr);

            var response = await _productService.UpdateStatusByIdAsync(id, updateDto.StatusId, userId);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        // GET: api/v1/product/5/status-history
        [HttpGet("{id}/status-history")]
        public async Task<ActionResult<ResponseDto<IEnumerable<ProductStatusHistoryDto>>>> GetProductStatusHistory(Guid id)
        {
            var response = await _productService.GetStatusHistoryAsync(id);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }

    public class StatusUpdateDto
    {
        public string StatusSlug { get; set; } = null!;
    }

    public class StatusUpdateByIdDto
    {
        public Guid StatusId { get; set; }
    }
}
