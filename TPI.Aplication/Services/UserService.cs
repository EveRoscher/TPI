using TPI.Aplication.Abstractions;
using TPI.Aplication.Abstractions.Infraestructure;
using TPI.Aplication.Exceptions;
using TPI.Aplication.Mappers;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;

namespace TPI.Aplication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserResponse>> GetAllAsync()
        {
            return (await _userRepository.GetAllAsync())
                .Select(x => x.ToUserResponse())
                .ToList();
        }

        public async Task<UserResponse> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new NotFoundException($"No se encontró un usuario con id '{id}'.");

            return user.ToUserResponse();
        }

        public async Task<UserResponse> CreateAsync(UserRequest request)
        {
            var newUser = request.ToUser();
            await _userRepository.AddAsync(newUser);
            return newUser.ToUserResponse();
        }

        public async Task UpdateAsync(UserRequest request, Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new NotFoundException($"No se encontró un usuario con id '{id}'.");

            user.Name = request.Name;
            user.Email = request.Email;
            user.Phone = request.Phone;
            user.Role = request.Role;

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new NotFoundException($"No se encontró un usuario con id '{id}'.");

            await _userRepository.DeleteAsync(id);
        }
    }
}
