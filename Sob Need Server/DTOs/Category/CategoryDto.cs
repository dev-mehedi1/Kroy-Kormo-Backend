namespace Sob_Need_Server.DTOs.Category
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public int? ParentCategoryId { get; set; }
    }
}
