using System;
using System.Collections.Generic;
using System.Text;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Domain.Entities;

namespace TPI.Aplication.Mappers
{
    public static class ProductMapper
    {
        public static ProductResponse ToProductResponse(this Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
        }

        public static Product ToProduct(this ProductRequests productRequest)
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                Name = productRequest.Name,
                Description = productRequest.Description,
                Price = productRequest.Price
            };
        }
    }
}
