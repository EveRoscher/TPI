using TPI.Domain.Entities;

namespace TPI.Aplication.Abstractions
{
    public interface IProductCategoryService
    {
        List<ProductCategory> getAll();
        ProductCategory getById(Guid id);
        ProductCategory create(ProductCategory productCategory);
        ProductCategory update(ProductCategory productCategory);
        bool delete(ProductCategory productCategory);
        object? GetAll();
    }
}



