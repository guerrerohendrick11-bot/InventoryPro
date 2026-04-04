using InventoryProBackend.Dto;

namespace InventoryProBackend.InterfaceService
{
    public interface IProducts
    {
        Task<IEnumerable<ProductsDto>> GetAllAsync(string? name, int? categoryId);
        Task<ProductsDto> GetByIdAsync(int id);
        Task<ProductsDto> CreateAsync(ProductsDto dto);
        Task<bool> UpdateAsync(int id, ProductsDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
