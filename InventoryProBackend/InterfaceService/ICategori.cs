using InventoryProBackend.Dto;

namespace InventoryProBackend.InterfaceService
{
    public interface ICategori
    {
        Task<IEnumerable<CategoriDto>> GetAllAsync();
        Task<CategoriDto> GetByIdAsync(int id);
        Task<CategoriDto> CreateAsync(CategoriDto dto);
        Task<bool> UpdateAsync(int id, CategoriDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
