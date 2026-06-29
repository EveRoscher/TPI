using TPI.Aplication.Requests;
using TPI.Aplication.Responses;

namespace TPI.Aplication.Abstractions
{
    public interface IProductCategoryService
    {
        Task<List<ProductCategoryResponse>> GetAllAsync();
        Task<ProductCategoryResponse> GetByIdAsync(Guid id);
        Task<ProductCategoryResponse> CreateAsync(ProductCategoryRequest productCategory);
        Task UpdateAsync(ProductCategoryRequest productCategory, Guid id);
        Task DeleteAsync(Guid id);
    }
}
