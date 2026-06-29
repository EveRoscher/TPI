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

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetAllAsync()
        {
            var users = await _userService.GetAllAsync();

            if (!users.Any())
                return NotFound("No hay usuarios registrados.");

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _userService.GetByIdAsync(id));
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateAsync([FromBody] UserRequest user)
        {
            var createdUser = await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdUser.Id }, createdUser);
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] UserRequest user, [FromRoute] Guid id)
        {
            await _userService.UpdateAsync(user, id);
            return NoContent();
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}