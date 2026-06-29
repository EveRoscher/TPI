using TPI.Aplication.Abstractions.Infraestructure;
using TPI.Domain.Entities;

namespace TPI.Infraestructure.Persistance.Repository
{
    public class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(TPIDbContext context) : base(context)
        {
        }
    }
}
