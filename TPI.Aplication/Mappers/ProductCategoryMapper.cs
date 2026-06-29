using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Domain.Entities;

namespace TPI.Aplication.Mappers
{
    public static class ProductCategoryMapper
    {
        public static ProductCategoryResponse ToProductCategoryResponse(this ProductCategory category)
        {
            return new ProductCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description ?? string.Empty
            };
        }

        public static ProductCategory ToProductCategory(this ProductCategoryRequest request)
        {
            return new ProductCategory
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow
            };
        }
    }
}
