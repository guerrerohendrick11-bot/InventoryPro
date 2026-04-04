using InventoryProBackend.Data;
using InventoryProBackend.Dto.Login;
using InventoryProBackend.InterfaceService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventoryProBackend.Service
{
    public class AuthService : IAuthService
    {
        private readonly ContextDb _context;
        private readonly IConfiguration _config;

        public AuthService(ContextDb context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            // 🔥 1. Traer usuario CON su rol
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == dto.Username.ToLower());

            // 🔥 2. Validaciones
            if (user == null || user.PasswordHash != dto.Password)
            {
                return null;
            }

            if (user.Role == null)
            {
                throw new Exception("El usuario no tiene rol asignado");
            }

            // 🔥 3. Claims (AQUÍ ESTÁ EL CAMBIO IMPORTANTE)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.Name) // 🔥 ahora sí correcto
            };

            // 🔐 4. Key
            var keySection = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(keySection))
            {
                throw new Exception("La clave JWT no está configurada en appsettings.json");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keySection));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 🎟️ 5. Token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(_config["Jwt:ExpiresInMinutes"] ?? "60")
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}