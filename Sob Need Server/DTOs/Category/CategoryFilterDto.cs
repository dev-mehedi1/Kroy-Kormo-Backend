namespace Sob_Need_Server.DTOs.Category
{
    public class CategoryFilterDto
    {
        public int? ParentCategoryId { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string? SearchText { get; set; }
        public bool? OrderByAsc{ get; set; }
    }
}
