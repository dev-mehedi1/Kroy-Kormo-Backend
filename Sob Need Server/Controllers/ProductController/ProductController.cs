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
        public async Task<ActionResult<ResponseDto<IEnumerable<ProductDto>>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(new ResponseDto<IEnumerable<ProductDto>>(true, "Products retrieved successfully", products));
        }

        // GET: api/product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<ProductDto>>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound(new ResponseDto<ProductDto>(false, "Product not found", null));
            }

            return Ok(new ResponseDto<ProductDto>(true, "Product retrieved successfully", product));
        }

        // POST: api/product
        [HttpPost]
        public async Task<ActionResult<ResponseDto<ProductDto>>> CreateProduct(ProductDto productDto)
        {
            var createdProduct = await _productService.CreateProductAsync(productDto);
            return CreatedAtAction(
                nameof(GetProductById),
                new { id = createdProduct.ProductId },
                new ResponseDto<ProductDto>(true, "Product created successfully", createdProduct)
            );
        }

        // PUT: api/product/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<ProductDto>>> UpdateProduct(int id, ProductDto productDto)
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, productDto);

            if (updatedProduct == null)
            {
                return NotFound(new ResponseDto<ProductDto>(false, "Product not found", null));
            }

            return Ok(new ResponseDto<ProductDto>(true, "Product updated successfully", updatedProduct));
        }

        // DELETE: api/product/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto<object>>> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProductAsync(id);

            if (!success)
            {
                return NotFound(new ResponseDto<object>(false, "Product not found", null));
            }

            return Ok(new ResponseDto<object>(true, "Product deleted successfully", null));
        }
    }
}
