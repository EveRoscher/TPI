using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Domain.Entities;

namespace TPI.Aplication.Mappers
{
    public static class UserMapper
    {
        public static UserResponse ToUserResponse(this User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Phone = user.Phone,
                Status = user.Status.ToString(),
                IsSuspect = user.IsSuspect,
                CreatedAt = user.CreatedAt,
                LastOrderAt = user.LastOrderAt,
                Role = user.Role
            };
        }

        public static User ToUser(this UserRequest request)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Password = request.Password, // Debería hashearse en el servicio
                Name = request.Name,
                Phone = request.Phone,
                Status = UserStatus.Active,
                IsSuspect = false,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow
            };
        }

        public static User ToUser(this SignUpRequest request)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Password = request.Password, // Debería hashearse en el servicio
                Name = request.Name,
                Phone = request.Phone,
                Status = UserStatus.Active,
                IsSuspect = false,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow
            };
        }
    }
}
