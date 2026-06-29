namespace TPI.Aplication.Requests
{
    public class ProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public string? ImageUrl { get; set; }
        public Guid ProductCategoryId { get; set; }
    }
}
