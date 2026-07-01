using Microsoft.EntityFrameworkCore;
using System.Linq;
using TPI.Aplication.Abstractions.Infraestructure;
using TPI.Domain.Entities;

namespace TPI.Infraestructure.Persistance.Repository
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(TPIDbContext context) : base(context)
        {
        }

        public async Task<List<Payment>> GetRejectedPaymentsAsync()
        {
            return await _context.Payments
                .Where(p => p.Status == PaymentStatus.Rejected && p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Payment?> GetByIdWithDeletedAsync(Guid id)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
