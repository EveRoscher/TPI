namespace TPI.Aplication.Responses
{
    public class OrderItemResponse
    {
        public Guid Id { get; set; }
        public string NameSnapshot { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => UnitPrice * Quantity;
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int? RemainingStock { get; set; }
        public decimal? OrderTotal { get; set; }
    }
}
