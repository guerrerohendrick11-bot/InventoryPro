using InventoryProBackend.Dto.Login;

namespace InventoryProBackend.InterfaceService
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDto dto);
    }
}
