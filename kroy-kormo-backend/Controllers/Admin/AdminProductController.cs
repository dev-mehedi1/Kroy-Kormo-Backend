using DTOs.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.Utils;

namespace Sob_Need_Server.Controllers.Admin
{
    [Route(Constants.BasePath + "/admin/products")]
    [ApiController]
    public class AdminProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<ProductDto>>>> GetProducts([FromQuery] string? status)
        {
            try
            {
                var query = _context.Products
                    .Include(p => p.User)
                    .Include(p => p.Category)
                    .Include(p => p.Status)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(p => p.Status.Slug == status);
                }

                var products = await query.Select(p => new ProductDto 
                {
                    ProductId = p.ProductId,
                    Title = p.Title,
                    Price = p.Price,
                    Category = p.Category.CategoryName,
                    CategoryId = p.CategoryId,
                    StatusName = p.Status.StatusName,
                    StatusSlug = p.Status.Slug, // Ensure StatusSlug is populated
                    SellerName = p.User.FullName,
                    CreatedAt = p.CreatedAt,
                    IsActive = p.IsActive,
                    ImageUrl = p.Attachments.Any() ? p.Attachments.First().ImageUrl : null
                }).ToListAsync();

                 return Ok(new ResponseDto<IEnumerable<ProductDto>>(true, "Products retrieved", products));
            }
             catch (Exception ex)
            {
                return BadRequest(new ResponseDto<IEnumerable<ProductDto>>(false, ex.Message, null!));
            }
        }

        [HttpPatch("{productId}/status")]
        public async Task<ActionResult<ResponseDto<bool>>> UpdateProductStatus(Guid productId, [FromBody] string statusSlug)
        {
             try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null) return NotFound(new ResponseDto<bool>(false, "Product not found", false));
                
                var status = await _context.ProductStatuses.FirstOrDefaultAsync(s => s.Slug == statusSlug);
                 if (status == null) return BadRequest(new ResponseDto<bool>(false, "Invalid status slug", false));

                product.StatusId = status.StatusId;
                // Automatically activate/deactivate based on status if needed
                product.IsActive = (statusSlug == "active"); 

                await _context.SaveChangesAsync();
                return Ok(new ResponseDto<bool>(true, $"Product status updated to {status.StatusName}", true));
            }
             catch (Exception ex)
            {
                return BadRequest(new ResponseDto<bool>(false, ex.Message, false));
            }
        }
    }
}
