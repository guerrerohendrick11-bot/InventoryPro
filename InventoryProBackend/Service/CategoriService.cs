using Microsoft.EntityFrameworkCore;
using InventoryProBackend.Data;
using InventoryProBackend.Dto;
using InventoryProBackend.InterfaceService;
using InventoryProBackend.Models;

namespace InventoryProBackend.Service
{
    public class CategoriService : ICategori
    {
        private readonly ContextDb _context;
       public CategoriService(ContextDb context)
        {
            _context = context;
        }



        public async Task<CategoriDto> CreateAsync(CategoriDto dto)
        {
            var category = new Categories
            {
                Name = dto.Name,
                Description = dto.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoriDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CategoriDto>> GetAllAsync()
        {
           return await _context.Categories.Select(c => new CategoriDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).
            ToListAsync();
        }

        public async Task<CategoriDto> GetByIdAsync(int id)
        {
            return await _context.Categories.Where(c => c.Id == id).Select(c => new CategoriDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(int id, CategoriDto dto)
        {
           return await _context.Categories.Where(c => c.Id == id).ExecuteUpdateAsync(c => c
                .SetProperty(p => p.Name, dto.Name)
                .SetProperty(p => p.Description, dto.Description)) > 0;
        }
    }
}
