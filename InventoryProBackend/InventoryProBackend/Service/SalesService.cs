using System;
using InventoryProBackend.Data;
using InventoryProBackend.Dto;
using InventoryProBackend.InterfaceService;
using InventoryProBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryProBackend.Service
{
    public class SalesService : ISaleService
    {
        private readonly ContextDb _context;

        public SalesService(ContextDb context)
        {
            _context = context;
        }

        public async Task<SalesDto> CreateAsync(SalesDto dto)
        {
            var sales = new Sales
            {
                CustomerId = dto.CustomerId,    
                Total = dto.Total,
                UsuerId = dto.UsuerId
            };

            await _context.Sales.AddAsync(sales);
            await _context.SaveChangesAsync();

            dto.Id = sales.Id;
            return dto;
        }

        public async Task<IEnumerable<SalesDto>> GetAllAsync()
        {
            return await _context.Sales
                .Select(s => new SalesDto
                {
                    Id = s.Id,
                    CustomerId = s.CustomerId,
                    Total = s.Total,
                    UsuerId = s.UsuerId,
                
                 
                })
                .ToListAsync();
        }

        public async Task<SalesDto?> GetByIdAsync(int id)
        {
            return await _context.Sales
                .Where(s => s.Id == id)
                .Select(s => new SalesDto
                {
                    Id = s.Id,
                    CustomerId = s.CustomerId,
                    Total = s.Total,
                    UsuerId = s.UsuerId
                })
                .FirstOrDefaultAsync();
        }
    }
}