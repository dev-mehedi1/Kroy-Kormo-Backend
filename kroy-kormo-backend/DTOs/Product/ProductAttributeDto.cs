namespace Sob_Need_Server.DTOs.Product
{
    public class ProductAttributeDto
    {
        public Guid AttributeId { get; set; }
        public string AttributeName { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public string DataType { get; set; } = "string";
        public bool IsRequired { get; set; }
        public int DisplayOrder { get; set; }
        public string? Options { get; set; }
    }
}
