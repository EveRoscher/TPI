using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Domain.Entities;

namespace TPI.Aplication.Abstractions
{
    public interface IProductService
    {
        Task<List<ProductResponse>> GetAllAsync();
        Task<ProductResponse> GetByIdAsync(Guid id);
        Task<ProductResponse> CreateAsync(ProductRequest product);
        Task UpdateAsync(ProductRequest product, Guid id);
        Task DeleteAsync(Guid id);
    }
}

