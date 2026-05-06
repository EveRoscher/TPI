using System;
using System.Collections.Generic;
using System.Linq;
using TPI.Aplication.Abstractions;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Aplication.Mappers;
using TPI.Domain.Entities;

namespace TPI.Aplication.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new();

        public ProductService()
        {
            // Inicializo con datos de ejemplo 
        }

        public List<ProductResponse> GetAll()
        {
            return _products
                .OrderBy(x => x.Price)
                .Select(x => x.ToProductResponse())
                .ToList();
        }

        public ProductResponse? GetById(Guid id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return product?.ToProductResponse();
        }

        public ProductResponse Create(ProductRequests product)
        {
            var newProduct = product.ToProduct();
            newProduct.Id = Guid.NewGuid();
            _products.Add(newProduct);
            return newProduct.ToProductResponse();
        }

        public bool Update(Product product)
        {
            var existing = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existing == null)
                return false;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            return true;
        }

        public bool Delete(Guid id)
        {
            var existing = _products.FirstOrDefault(p => p.Id == id);
            if (existing == null)
                return false;

            _products.Remove(existing);
            return true;
        }

        public bool Update(ProductRequests product, Guid id)
        {
            var existing = _products.FirstOrDefault(p => p.Id == id);
            if (existing == null)
                return false;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            return true;
        }

        public void Update(ProductResponse productToUpdate)
        {
            var existing = _products.FirstOrDefault(p => p.Id == productToUpdate.Id);
            if (existing == null)
                return;

            existing.Name = productToUpdate.Name;
            existing.Description = productToUpdate.Description;
            existing.Price = productToUpdate.Price;
        }
    }
}



