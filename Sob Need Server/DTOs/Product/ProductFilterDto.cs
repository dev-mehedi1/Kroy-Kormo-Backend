namespace Sob_Need_Server.DTOs.Product
{
    public class ProductFilterDto
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string? SearchText { get; set; }
        public bool? OrderByAsc { get; set; }
        public string? CategorySlug{ get; set; }
        public bool? IsTop{ get; set; }
        public bool? IsArgent{ get; set; }
    }
}
