using InventoryProBackend.Dto;
using InventoryProBackend.InterfaceService;
using Microsoft.AspNetCore.Mvc;

namespace InventoryProBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

     
        /// Obtiene la lista de todos los usuarios.
 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersDto>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }


        /// Obtiene un usuario por su ID.
     
        [HttpGet("{id}")]
        public async Task<ActionResult<UsersDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound("Usuario no encontrado.");
            return Ok(user);
        }


        /// Registra un nuevo usuario en el sistema.

        [HttpPost]
        public async Task<ActionResult<UsersDto>> Create(UsersDto dto)
        {
            if (string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.PasswordHash))
            {
                return BadRequest("El nombre de usuario y la contraseña son obligatorios.");
            }

            var result = await _userService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }


        /// Actualiza los datos de un usuario.

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UsersDto dto)
        {
            var success = await _userService.UpdateAsync(id, dto);
            if (!success) return NotFound("No se pudo actualizar el usuario.");

            return NoContent();
        }

        
        /// Elimina (o desactiva) un usuario del sistema.
    
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _userService.DeleteAsync(id);
            if (!success) return NotFound("El usuario no existe.");

            return NoContent();
        }
    }
}