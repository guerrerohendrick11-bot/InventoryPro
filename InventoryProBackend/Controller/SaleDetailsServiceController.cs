using InventoryProBackend.Dto;

using InventoryProBackend.InterfaceService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryProBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SaleDetailsController : ControllerBase
    {
       
        private readonly ISaleDetailsService _saleDetailsService;

        public SaleDetailsController(ISaleDetailsService saleDetailsService)
        {
            _saleDetailsService = saleDetailsService;
        }

      
        /// Obtiene todos los artículos de una venta específica.
      
        /// < name="saleId">ID de la venta principal
        [HttpGet("by-sale/{saleId}")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<ActionResult<IEnumerable<SaleDetailsDto>>> GetBySaleId(int saleId)
        {
            var details = await _saleDetailsService.GetBySaleIdAsync(saleId);
            return Ok(details);
        }

        
        /// Obtiene un detalle de línea específico por su propio ID.
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<ActionResult<SaleDetailsDto>> GetById(int id)
        {
            var detail = await _saleDetailsService.GetByIdAsync(id);
            if (detail == null) return NotFound("Detalle de venta no encontrado.");

            return Ok(detail);
        }


        /// Agrega un producto a una venta.
        [HttpPost]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<ActionResult<SaleDetailsDto>> Create(SaleDetailsDto dto)
        {
            try
            {
                if (dto.Quantity <= 0)
                    return BadRequest("La cantidad debe ser mayor a cero.");

                var result = await _saleDetailsService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// Actualiza un detalle de venta (Ej: cambiar la cantidad o el precio).

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, SaleDetailsDto dto)
        {
            var success = await _saleDetailsService.UpdateAsync(id, dto);
            if (!success) return NotFound("No se pudo actualizar el detalle.");

            return NoContent();
        }

     
        /// Elimina un producto de la venta.

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _saleDetailsService.DeleteAsync(id);
            if (!success) return NotFound("El detalle de venta no existe.");

            return NoContent();
        }
    }
}