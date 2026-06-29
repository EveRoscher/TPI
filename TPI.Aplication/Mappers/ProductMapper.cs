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
                Price = product.Price,
                IsActive = product.IsActive,
                ImageUrl = product.ImageUrl,
                ProductCategoryId = product.ProductCategoryId
            };
        }

        public static Product ToProduct(this ProductRequest productRequest)
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                Name = productRequest.Name,
                Description = productRequest.Description,
                Price = productRequest.Price,
                IsActive = productRequest.IsActive,
                ImageUrl = productRequest.ImageUrl,
                ProductCategoryId = productRequest.ProductCategoryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow
            };
        }
    }
}
