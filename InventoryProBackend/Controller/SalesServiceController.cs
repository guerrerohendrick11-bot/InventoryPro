using InventoryProBackend.Dto;
using InventoryProBackend.InterfaceService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryProBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _salesService;

        public SalesController(ISaleService salesService)
        {
            _salesService = salesService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<ActionResult<IEnumerable<SalesDto>>> GetAll()
        {
            return Ok(await _salesService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<ActionResult<SalesDto>> GetById(int id)
        {
            var sale = await _salesService.GetByIdAsync(id);
            if (sale == null) return NotFound("Venta no encontrada.");
            return Ok(sale);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<ActionResult<SalesDto>> Create(CreateSaleDto dto)
        {
            try
            {
                var result = await _salesService.CreateWithDetailsAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}