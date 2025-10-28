namespace Sob_Need_Server.DTOs.Category
{
    public class CategoryDto
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public Guid? ParentCategoryId { get; set; }
        public int ProductCount { get; set; }
    }
}
