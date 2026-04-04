using InventoryProBackend.Data;
using Microsoft.EntityFrameworkCore;
using InventoryProBackend.Dto;
using InventoryProBackend.InterfaceService;
using InventoryProBackend.Models;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Wasm;

namespace InventoryProBackend.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ContextDb _context;
        public CustomerService(ContextDb context)
        {
            _context = context;
        }

        public async Task<CustomerDto> CreateAsync(CustomerDto dto)
        {
            var customer = new Customers 
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync(); 

            dto.Id = customer.Id; 
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Customers.FindAsync(id);
            if (category == null )
                return false; 
            _context.Customers.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            return await _context.Customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone
            }).ToListAsync();
        }

        public async Task<CustomerDto> GetByIdAsync(int id)
        {
            return await _context.Customers.Where(c => c.Id == id).Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone
            }).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(int id, CustomerDto dto)
        {
            return await _context.Customers.Where(c => c.Id == id).ExecuteUpdateAsync(c => c
                .SetProperty(c => c.Name, dto.Name)
                .SetProperty(c => c.Email, dto.Email)
                .SetProperty(c => c.Phone, dto.Phone)) > 0;
        }
    }
}
