using System;
using System.Collections.Generic;
using System.Text;

namespace TPI.Aplication.Requests
{
    public class ProductRequests
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
