using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using TPI.Aplication.Abstractions;
using TPI.Aplication.Exceptions;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Domain.Entities;
using TPI.Infraestructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TPI.Infraestructure.ExternalServices
{
    public class AuthService : IAuthService
    {
        private readonly TPIDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(TPIDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public AuthResponse SignUp(SignUpRequest request)
        {
            if (!Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ValidationException($"El email '{request.Email}' no tiene un formato válido.");

            bool emailEnUso = _context.Users.Any(u => u.Email == request.Email);

            if (emailEnUso)
                throw new ConflictException($"El email '{request.Email}' ya está registrado.");

            string contrasenaHasheada = BCrypt.Net.BCrypt.HashPassword(request.Password);
            Guid nuevoId = Guid.NewGuid();
            string rol = request.Role == "Admin" ? "Admin" : "Cliente";

            var user = new User
            {
                Id = nuevoId,
                Name = request.Name,
                Email = request.Email,
                Password = contrasenaHasheada,
                Phone = request.Phone,
                Role = rol,
                CreatedAt = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow,
                Status = UserStatus.Active,
                IsSuspect = false,
            };

            _context.Users.Add(user);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException("Error al guardar el usuario en la base de datos.", ex);
            }

            return new AuthResponse
            {
                Token = GenerarToken(nuevoId, request.Email, rol),
                Role = rol,
                UserId = nuevoId,
                Email = request.Email
            };
        }

        public AuthResponse SignIn(SignInRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
                throw new UnauthorizedException("Credenciales incorrectas.");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                throw new UnauthorizedException("Credenciales incorrectas.");

            return new AuthResponse
            {
                Token = GenerarToken(user.Id, request.Email, user.Role),
                Role = user.Role,
                UserId = user.Id,
                Email = request.Email
            };
        }

        private string GenerarToken(Guid userId, string email, string rol)
        {
            string key = _configuration["Jwt:Key"]!;
            string issuer = _configuration["Jwt:Issuer"]!;
            string audience = _configuration["Jwt:Audience"]!;
            int expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"]!);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, rol),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
