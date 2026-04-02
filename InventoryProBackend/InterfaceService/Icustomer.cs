using InventoryProBackend.Dto;

namespace InventoryProBackend.InterfaceService
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllAsync();
        Task<CustomerDto> GetByIdAsync(int id);
        Task<CustomerDto> CreateAsync(CustomerDto dto);
        Task<bool> UpdateAsync(int id, CustomerDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
