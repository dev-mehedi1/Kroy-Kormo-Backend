using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Models.Products;

namespace Sob_Need_Server.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
    }
}
