using TPI.Aplication.Abstractions;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Domain.Entities;
namespace TPI.Aplication.Services

{
    public class ProductService : IProductService

    {
        private static readonly List<Product> _products = new();

  

        public List<ProductResponse> GetAll()
        { 
        return _products
       .OrderBy(x => x.Price)
       .Select(x => new ProductResponse
       {
           Id = x.Id,
           Name = x.Name,
           Description = x.Description,
           Price = x.Price
       })
       .ToList();
        }

        public ProductResponse? GetById(Guid id)
        {
            return _products
       .Select(x => new ProductResponse
       {
           Id = x.Id,
           Name = x.Name,
           Description = x.Description,
           Price = x.Price
       })
       .FirstOrDefault();
        }
         
        public ProductResponse Create(ProductRequests product)
        {
            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
                        _products.Add(newProduct);


                        return new ProductResponse

                        {
                            Id = newProduct.Id,
                            Name = newProduct.Name,
                            Description = newProduct.Description,
                            Price = newProduct.Price
                        };
        }   

        public Product Update(Product product)
        {
            throw new NotImplementedException();
        }

      
        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        
    }
}



