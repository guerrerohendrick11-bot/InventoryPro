using InventoryProBackend.Data;
using Microsoft.EntityFrameworkCore;
using InventoryProBackend.Dto;
using InventoryProBackend.InterfaceService;
using InventoryProBackend.Models;

namespace InventoryProBackend.Service
   
{
    // Servicio encargado de la gestión lógica de los productos.
    public class ProducService : IProducts
    {
        private readonly ContextDb _context;
        public ProducService(ContextDb context)
        {
            _context = context;
        }

        // Crea un nuevo producto en la base de datos.
        public async Task<ProductsDto> CreateAsync(ProductsDto dto)
        {
            var product = new Products
            {
                Name = dto.Name,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId,
                IsActive = dto.IsActive
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            dto.Id = product.Id;
           return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        // Obtiene una lista filtrada de productos.
        public async Task<IEnumerable<ProductsDto>> GetAllAsync(string? name, int? categoryId)
        {
            return await _context.Products
                .Where(p => (string.IsNullOrEmpty(name) || p.Name.Contains(name)) &&
                            (!categoryId.HasValue || p.CategoryId == categoryId.Value))
                .Select(p => new ProductsDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId,
                    IsActive = p.IsActive
                })
                .ToListAsync();
        }

        public async Task<ProductsDto> GetByIdAsync(int id)
        {
            return await _context.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductsDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId,
                    IsActive = p.IsActive
                })
                .FirstOrDefaultAsync();
        }

        // Actualiza los datos de un producto existente usando una consulta directa a la DB.
        public async Task<bool> UpdateAsync(int id, ProductsDto dto)
        {
            return await _context.Products.Where(p => p.Id == id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(p => p.Name, dto.Name)
                    .SetProperty(p => p.Price, dto.Price)
                    .SetProperty(p => p.Stock, dto.Stock)
                    .SetProperty(p => p.CategoryId, dto.CategoryId)
                    .SetProperty(p => p.IsActive, dto.IsActive)) > 0;
        }
    }
}
