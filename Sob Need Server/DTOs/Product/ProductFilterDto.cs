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
        public string? UserId{ get; set; }
        public int? CountryId{ get; set; }
        public int? CityId{ get; set; }
        public int? RegionId{ get; set; }
        public int? StateId{ get; set; }
    }
}
