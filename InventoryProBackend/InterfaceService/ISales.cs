using InventoryProBackend.Dto;

namespace InventoryProBackend.InterfaceService
{
    public interface ISaleService
    {
        Task<IEnumerable<SalesDto>> GetAllAsync();
        Task<SalesDto> GetByIdAsync(int id);
        Task<SalesDto> CreateAsync(SalesDto dto);
    }
}
