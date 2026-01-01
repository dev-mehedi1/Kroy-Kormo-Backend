namespace Sob_Need_Server.DTOs.Product
{
    public class ConditionFilterDto
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string? SearchText { get; set; }
        public bool? OrderByAsc { get; set; }
    }
}
