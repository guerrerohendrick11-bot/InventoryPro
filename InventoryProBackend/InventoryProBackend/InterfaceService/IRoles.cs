using InventoryProBackend.Dto;

namespace InventoryProBackend.InterfaceService
{
    public interface IRoleService
    {
        Task<IEnumerable<RolesDto>> GetAllAsync();
        Task<RolesDto> GetByIdAsync(int id);
        Task<RolesDto> CreateAsync(RolesDto dto);
        Task<bool> UpdateAsync(int id, RolesDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
