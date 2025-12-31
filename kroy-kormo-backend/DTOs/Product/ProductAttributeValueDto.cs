namespace DTOs.Product
{
    public class ProductAttributeValueDto
    {
        public Guid ValueId { get; set; }
        public Guid AttributeId { get; set; }
        public string AttributeName { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}
