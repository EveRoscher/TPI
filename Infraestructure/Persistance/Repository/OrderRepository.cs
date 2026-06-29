using TPI.Aplication.Abstractions.Infraestructure;
using TPI.Domain.Entities;

namespace TPI.Infraestructure.Persistance.Repository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(TPIDbContext context) : base(context)
        {
        }
    }
}
