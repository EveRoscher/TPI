using Microsoft.EntityFrameworkCore;
using TPI.Aplication.Abstractions.Infraestructure;
using TPI.Domain.Entities;

namespace TPI.Infraestructure.Persistance.Repository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(TPIDbContext context) : base(context)
        {
        }

        public async Task<Order?> GetOrderWithItemsByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<List<Order>> GetOrdersWithItemsAsync()
        {
            return await _dbSet
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderWithPaymentByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(o => o.Payment)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }
    }
}
