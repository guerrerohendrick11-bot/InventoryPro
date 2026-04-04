using InventoryProBackend.Data;
using InventoryProBackend.Dto;
using InventoryProBackend.InterfaceService;
using InventoryProBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryProBackend.Service
{
    public class RolesService : IRoleService
    {
        private readonly ContextDb _context;
        public RolesService(ContextDb context)
        {
            _context = context;
        }

        public async Task<RolesDto> CreateAsync(RolesDto dto)
        {
            var role = new Roles
            {
                Name = dto.Name
            };

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            dto.Id = role.Id;
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var roles = await _context.Roles.FindAsync(id);
            if (roles == null) return false;

            _context.Roles.Remove(roles);
              await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RolesDto>> GetAllAsync()
        {
            return await _context.Roles
                .Select(r => new RolesDto
                {
                    Id = r.Id,
                    Name = r.Name
                }).ToListAsync();
        }

        public async Task<RolesDto> GetByIdAsync(int id)
        {
            return await _context.Roles.Where(r => r.Id == id).Select(r => new RolesDto
            {
                Id = r.Id,
                Name = r.Name
            }).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(int id, RolesDto dto)
        {
            return await _context.Roles.Where(r => r.Id == id)
                .ExecuteUpdateAsync(r => r.SetProperty
                (role => role.Name, dto.Name)) > 0;
        }
    }
}
