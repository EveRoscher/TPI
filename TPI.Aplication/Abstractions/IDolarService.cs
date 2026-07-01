using System.Threading.Tasks;

namespace TPI.Aplication.Abstractions
{
    public interface IDolarService
    {
        Task<decimal> GetOfficialBuyRateAsync();
    }
}
