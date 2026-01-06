namespace DTOs.Category;

public class CategoryDto
{
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public int ProductCount { get; set; }
    public List<CategoryDto> Children { get; set; } = new();
}

