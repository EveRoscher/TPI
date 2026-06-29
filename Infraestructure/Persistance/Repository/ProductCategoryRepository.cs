using TPI.Aplication.Abstractions.Infraestructure;
using TPI.Domain.Entities;

namespace TPI.Infraestructure.Persistance.Repository
{
    public class ProductCategoryRepository : BaseRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(TPIDbContext context) : base(context)
        {
        }
    }
}
