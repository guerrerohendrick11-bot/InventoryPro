using InventoryProBackend.Dto;
using InventoryProBackend.InterfaceService;
using Microsoft.AspNetCore.Mvc;

namespace InventoryProBackend.Controllers
{
    [Route("api/[controller]")] // La ruta será: api/Roles
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        // Inyectamos el servicio a través del constructor
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        
        /// Obtiene la lista de todos los roles registrados.
    
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolesDto>>> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

       
        /// Obtiene un rol específico por su ID.
     
        [HttpGet("{id}")]
        public async Task<ActionResult<RolesDto>> GetById(int id)
        {
            var role = await _roleService.GetByIdAsync(id);

            if (role == null)
            {
                return NotFound($"No se encontró el rol con ID {id}");
            }

            return Ok(role);
        }

      
        /// Crea un nuevo rol (Ej: "Admin", "Vendedor").
    
        [HttpPost]
        public async Task<ActionResult<RolesDto>> Create(RolesDto dto)
        {
            // Validamos que el nombre no venga vacío
            if (string.IsNullOrEmpty(dto.Name))
            {
                return BadRequest("El nombre del rol es obligatorio.");
            }

            var nuevoRol = await _roleService.CreateAsync(dto);

            // Retorna un código 201 Created y la ruta para consultar el nuevo objeto
            return CreatedAtAction(nameof(GetById), new { id = nuevoRol.Id }, nuevoRol);
        }

       
        /// Actualiza el nombre de un rol existente.
  
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RolesDto dto)
        {
            var actualizado = await _roleService.UpdateAsync(id, dto);

            if (!actualizado)
            {
                return NotFound("No se pudo actualizar porque el rol no existe.");
            }

            return NoContent(); // 204 No Content es el estándar para Updates exitosos
        }

       
        /// Elimina un rol de la base de datos.
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _roleService.DeleteAsync(id);

            if (!eliminado)
            {
                return NotFound("El rol que intenta eliminar no existe.");
            }

            return NoContent();
        }
    }
}