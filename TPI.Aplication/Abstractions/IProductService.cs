using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Domain.Entities;

namespace TPI.Aplication.Abstractions
{
    public interface IProductService
    {
        List<ProductResponse> GetAll();
        ProductResponse GetById(Guid id);
        ProductResponse Create(ProductRequest product);
        void Update(ProductRequest product, Guid id);
        void Delete(Guid id);
    }
}

