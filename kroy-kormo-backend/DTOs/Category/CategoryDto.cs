namespace DTOs.Category;

public class CategoryDto
{
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Slug { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string? Type { get; set; } // "Marketplace" or "Job"
    public int ProductCount { get; set; }
    public List<CategoryDto> Children { get; set; } = new();
}

