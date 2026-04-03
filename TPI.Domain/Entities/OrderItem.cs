namespace TPI.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public string NameProduct { get; set; } = string.Empty;
        public decimal Price { get; set; }

    }
}
