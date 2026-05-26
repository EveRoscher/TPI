namespace TPI.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid Id { get; set; }
        public string NameProduct { get; set; } = string.Empty;
        public decimal Price { get; set; }

    }
}
