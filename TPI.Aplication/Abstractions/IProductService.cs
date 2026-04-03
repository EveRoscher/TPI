using TPI.Domain.Entities;

namespace TPI.Aplication.Abstractions
{
    public interface IProductService
    {
        List<Product> getAll();
        Product getById(Guid id);
        Product create(Product product);
        Product update(Product product);
        bool delete(Product product);
        object? GetAll();
    }
}
