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

        public async Task<List<ProductResponse>> GetAllAsync()
        {
            return (await _productRepository.GetAllAsync())
                .OrderBy(x => x.Price)
                .Select(x => x.ToProductResponse())
                .ToList();
        }

        public async Task<ProductResponse> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new NotFoundException($"No se encontró un producto con id '{id}'.");

            return product.ToProductResponse();
        }

        public async Task<ProductResponse> CreateAsync(ProductRequest product)
        {
            var newProduct = product.ToProduct();
            await _productRepository.AddAsync(newProduct);
            return newProduct.ToProductResponse();
        }


        public async Task DeleteAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new NotFoundException($"No se encontró un producto con id '{id}'.");

            await _productRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(ProductRequest product, Guid id)
        {
            var productToUpdate = await _productRepository.GetByIdAsync(id);

            if (productToUpdate == null)
                throw new NotFoundException($"No se encontró un producto con id '{id}'.");

            productToUpdate.Name = product.Name;
            productToUpdate.Description = product.Description;
            productToUpdate.Price = product.Price;
            productToUpdate.IsActive = product.IsActive;
            productToUpdate.Stock = product.Stock;
            productToUpdate.ImageUrl = product.ImageUrl;
            productToUpdate.ProductCategoryId = product.ProductCategoryId;

            await _productRepository.UpdateAsync(productToUpdate);
        }


    }
}
