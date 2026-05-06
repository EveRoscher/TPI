using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Domain.Entities;

namespace TPI.Aplication.Abstractions
{
    public interface IProductService
    {
        List<ProductResponse> GetAll();
        ProductResponse? GetById(Guid id);
        ProductResponse Create(ProductRequests product);
        Product Update(Product product);
        bool Delete(Guid id);
    }
}
