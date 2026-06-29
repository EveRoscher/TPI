using TPI.Aplication.Requests;
using TPI.Aplication.Responses;

namespace TPI.Aplication.Abstractions
{
    public interface IUserService
    {
        Task<List<UserResponse>> GetAllAsync();
        Task<UserResponse> GetByIdAsync(Guid id);
        Task<UserResponse> CreateAsync(UserRequest user);
        Task UpdateAsync(UserRequest user, Guid id);
        Task DeleteAsync(Guid id);
    }
}
