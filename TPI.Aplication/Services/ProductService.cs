using System;
using System.Collections.Generic;
using System.Linq;
using TPI.Aplication.Abstractions;
using TPI.Aplication.Abstractions.Infraestructure;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Aplication.Mappers;
using TPI.Domain.Entities;
using TPI.Aplication.Exceptions;

namespace TPI.Aplication.Services
{
    public class ProductService : IProductService 
    {

        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<ProductResponse> GetAll()
        {
            return _productRepository
                .GetAll()
                .OrderBy(x => x.Price)
                .Select(x => x.ToProductResponse())
                .ToList();
        }

        public ProductResponse GetById(Guid id)
        {
            var product = _productRepository.GetById(id);

            if (product == null)
                throw new NotFoundException($"No se encontró un producto con id '{id}'.");

            return product.ToProductResponse();
        }

        public ProductResponse Create(ProductRequest product)
        {
            var newProduct = product.ToProduct();
            _productRepository.Add(newProduct);
            return newProduct.ToProductResponse();
        }

    
        public void Delete(Guid id)
        {
            var product = _productRepository.GetById(id);

            if (product == null)
                throw new NotFoundException($"No se encontró un producto con id '{id}'.");

            _productRepository.Delete(id);
        }

        public void Update(ProductRequest product, Guid id)
        {
            var productToUpdate = _productRepository.GetById(id);

            if (productToUpdate == null)
                throw new NotFoundException($"No se encontró un producto con id '{id}'.");

            productToUpdate.Name = product.Name;
            productToUpdate.Description = product.Description;
            productToUpdate.Price = product.Price;

            _productRepository.Update(productToUpdate);
        }

        
    }
}




