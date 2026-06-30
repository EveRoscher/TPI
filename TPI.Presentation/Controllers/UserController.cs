using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPI.Aplication.Abstractions;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Presentation.Authorization;

namespace TPI.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Obtiene todos los usuarios registrados en el sistema (Solo Admin).
        /// </summary>
        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetAllAsync()
        {
            var users = await _userService.GetAllAsync();

            if (!users.Any())
                return NotFound("No hay usuarios registrados.");

            return Ok(users);
        }

        /// <summary>
        /// Obtiene un usuario específico por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único del usuario.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _userService.GetByIdAsync(id));
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema (Solo Admin).
        /// </summary>
        /// <param name="user">Datos del usuario a registrar (nombre, email, rol, etc.).</param>
        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateAsync([FromBody] UserRequest user)
        {
            var createdUser = await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdUser.Id }, createdUser);
        }

        /// <summary>
        /// Actualiza la información de un usuario existente (Solo Admin).
        /// </summary>
        /// <param name="user">Nuevos datos del usuario.</param>
        /// <param name="id">Identificador único del usuario a actualizar.</param>
        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] UserRequest user, [FromRoute] Guid id)
        {
            await _userService.UpdateAsync(user, id);
            return NoContent();
        }

        /// <summary>
        /// Elimina un usuario del sistema (Solo Admin).
        /// </summary>
        /// <param name="id">Identificador único del usuario a eliminar.</param>
        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}