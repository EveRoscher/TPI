namespace TPI.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string NameSnapshot { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        // 
        public Order Order { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
