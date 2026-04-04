using InventoryProBackend.Data;
using InventoryProBackend.Dto;
using InventoryProBackend.InterfaceService;
using InventoryProBackend.Models;
using Microsoft.EntityFrameworkCore;



namespace InventoryProBackend.Service
{
    public class SaleDetailsService : ISaleDetailsService
    {
        private readonly ContextDb _context;
        public SaleDetailsService(ContextDb context)
        {
            _context = context;
        }

        public async Task<SaleDetailsDto> CreateAsync(SaleDetailsDto dto)
        {
            var saleDetailsEntity = new SaleDetails
            {
                SaleId = dto.SaleId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                Price = (int)dto.Price
            };

            await _context.SaleDetails.AddAsync(saleDetailsEntity);
            await _context.SaveChangesAsync();

            dto.Id = saleDetailsEntity.Id;
            return dto;
        }

        
        public async Task<SaleDetailsDto?> GetByIdAsync(int id)
        {
            return await _context.SaleDetails
                .Where(s => s.Id == id) 
                .Select(s => new SaleDetailsDto
                {
                    Id = s.Id,
                    SaleId = s.SaleId,
                    ProductId = s.ProductId,
                    Quantity = s.Quantity,
                    Price = s.Price,
                    Subtotal = s.Quantity * s.Price
                })
                .FirstOrDefaultAsync(); 
        }

        public async Task<IEnumerable<SaleDetailsDto>> GetBySaleIdAsync(int saleId)
        {
            return await _context.SaleDetails
                .Where(s => s.SaleId == saleId)
                .Select(s => new SaleDetailsDto
                {
                    Id = s.Id,
                    SaleId = s.SaleId,
                    ProductId = s.ProductId,
                    Quantity = s.Quantity,
                    Price = s.Price,
                    Subtotal = s.Quantity * s.Price
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(int id, SaleDetailsDto dto)
        {
            return await _context.SaleDetails
                .Where(s => s.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.SaleId, dto.SaleId)
                    .SetProperty(p => p.ProductId, dto.ProductId)
                    .SetProperty(p => p.Quantity, dto.Quantity)
                    .SetProperty(p => p.Price, (int)dto.Price)) > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var saleDetails = await _context.SaleDetails.FindAsync(id);
            if (saleDetails == null) return false;

            _context.SaleDetails.Remove(saleDetails);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
