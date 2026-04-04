using System;
using InventoryProBackend.Data;
using InventoryProBackend.Dto;
using InventoryProBackend.InterfaceService;
using InventoryProBackend.Models;
using Microsoft.EntityFrameworkCore;
namespace InventoryProBackend.Service
{
    public class UserService : IUserService
    {
        private readonly ContextDb _context;

        public UserService(ContextDb context)
        {
            _context = context; 
        }

        public async Task<UsersDto> CreateAsync(UsersDto dto)
        {
            var user = new Users
            {
                Username = dto.Username,
                PasswordHash = dto.PasswordHash, 
                RoleId = dto.RoleId,
                IsActive = true 
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            dto.Id = user.Id;
            dto.IsActive = user.IsActive;
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
          
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UsersDto>> GetAllAsync()
        {
            return await _context.Users
                .Select(u => new UsersDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    RoleId = u.RoleId,
                    IsActive = u.IsActive
                }).ToListAsync();
        }

        public async Task<UsersDto?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UsersDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    RoleId = u.RoleId,
                    IsActive = u.IsActive
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(int id, UsersDto dto)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(p => p.Username, dto.Username)
                    .SetProperty(p => p.PasswordHash, dto.PasswordHash)
                    .SetProperty(p => p.RoleId, dto.RoleId)
                    .SetProperty(p => p.IsActive, dto.IsActive)) > 0;
        }
    }
}
