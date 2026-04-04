using InventoryProBackend.Dto;

namespace InventoryProBackend.InterfaceService
{
    public interface ISaleDetailsService
    {
        Task<IEnumerable<SaleDetailsDto>> GetBySaleIdAsync(int saleId);
        Task<SaleDetailsDto> GetByIdAsync(int id);
        Task<SaleDetailsDto> CreateAsync(SaleDetailsDto dto);
        Task<bool> UpdateAsync(int id, SaleDetailsDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
