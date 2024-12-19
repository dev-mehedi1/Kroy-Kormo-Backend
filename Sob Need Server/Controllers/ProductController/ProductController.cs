using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.Interface;
using Sob_Need_Server.Utils;
using Sob_Need_Server.DTOs.Common;

namespace Sob_Need_Server.Controllers.ProductController
{
    [Route(Constants.BasePath + "/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        // GET: api/product
        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<ProductDto>>>> GetAllProducts([FromQuery] ProductFilterDto Filters)
        {
            var response = await _productService.GetAllProductsAsync(Filters);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        // GET: api/product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<ProductDto>>> GetProductById(int id)
        {
            var response = await _productService.GetByIdAsync(id);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        // POST: api/product
        [HttpPost]
        public async Task<ActionResult<ResponseDto<ProductDto>>> CreateProduct(ProductDto productDto)
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
        public async Task<ActionResult<ResponseDto<ProductDto>>> UpdateProduct(int id, ProductDto productDto)
        {
            var response = await _productService.UpdateAsync(id, productDto);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
