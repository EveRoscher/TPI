using System;
using System.Collections.Generic;

namespace TPI.Aplication.Responses
{
    public class OrderDolaresResponse
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmountPesos { get; set; }
        public decimal TotalAmountDolares { get; set; }
        public decimal CotizacionUsada { get; set; }
        public List<ProductDolaresResponse> Products { get; set; } = [];
    }

    public class ProductDolaresResponse
    {
        public string Name { get; set; } = string.Empty;
        public decimal PrecioPesos { get; set; }
        public decimal PrecioDolares { get; set; }
        public int Quantity { get; set; }
    }
}
