using TPI.Aplication.Abstractions;
using TPI.Aplication.Abstractions.Infraestructure;
using TPI.Aplication.Exceptions;
using TPI.Aplication.Mappers;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;

namespace TPI.Aplication.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<List<ProductCategoryResponse>> GetAllAsync()
        {
            return (await _productCategoryRepository.GetAllAsync())
                .OrderBy(x => x.Name)
                .Select(x => x.ToProductCategoryResponse())
                .ToList();
        }

        public async Task<ProductCategoryResponse> GetByIdAsync(Guid id)
        {
            var category = await _productCategoryRepository.GetByIdAsync(id);

            if (category == null)
                throw new NotFoundException($"No se encontró una categoría con id '{id}'.");

            return category.ToProductCategoryResponse();
        }

        public async Task<ProductCategoryResponse> CreateAsync(ProductCategoryRequest request)
        {
            var newCategory = request.ToProductCategory();
            await _productCategoryRepository.AddAsync(newCategory);
            return newCategory.ToProductCategoryResponse();
        }

        public async Task UpdateAsync(ProductCategoryRequest request, Guid id)
        {
            var category = await _productCategoryRepository.GetByIdAsync(id);

            if (category == null)
                throw new NotFoundException($"No se encontró una categoría con id '{id}'.");

            category.Name = request.Name;
            category.Description = request.Description;

            await _productCategoryRepository.UpdateAsync(category);
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _productCategoryRepository.GetByIdAsync(id);

            if (category == null)
                throw new NotFoundException($"No se encontró una categoría con id '{id}'.");

            await _productCategoryRepository.DeleteAsync(id);
        }
    }
}
