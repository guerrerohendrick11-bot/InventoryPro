using InventoryProBackend.Dto;
using Microsoft.AspNetCore.Authorization;
using InventoryProBackend.InterfaceService;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProducts _productService;

    public ProductController(IProducts productService)  
    {
        _productService = productService;
    }

    // GET: api/Product?name=laptop&categoryId=1
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductsDto>>> GetAll([FromQuery] string? name, [FromQuery] int? categoryId)
    {
        var products = await _productService.GetAllAsync(name, categoryId);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductsDto>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductsDto>> Create(ProductsDto dto)
    {
        var result = await _productService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductsDto dto)
    {
        var success = await _productService.UpdateAsync(id, dto);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _productService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}