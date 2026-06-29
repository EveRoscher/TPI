using TPI.Aplication.Abstractions.Infraestructure;
using TPI.Domain.Entities;

namespace TPI.Infraestructure.Persistance.Repository
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(TPIDbContext context) : base(context)
        {
        }
    }
}
