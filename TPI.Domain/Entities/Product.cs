namespace TPI.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Guid ProductCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }

        // 
        public ProductCategory ProductCategory { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
