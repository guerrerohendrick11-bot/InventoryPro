using InventoryProBackend.Dto;

namespace InventoryProBackend.InterfaceService
{
    public interface IUserService
    {
        Task<IEnumerable<UsersDto>> GetAllAsync();
        Task<UsersDto> GetByIdAsync(int id);
        Task<UsersDto> CreateAsync(UsersDto dto);
        Task<bool> UpdateAsync(int id, UsersDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
