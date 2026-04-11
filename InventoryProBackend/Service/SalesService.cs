using InventoryProBackend.Data;
using InventoryProBackend.Dto;
using InventoryProBackend.InterfaceService;
using InventoryProBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryProBackend.Service
{
    public class SalesService : ISaleService
    {
        private readonly ContextDb _context;

        public SalesService(ContextDb context)
        {
            _context = context;
        }

        public async Task<SalesDto> CreateAsync(SalesDto dto)
        {
            var sales = new Sales
            {
                CustomerId = dto.CustomerId,
                Total = (int)dto.Total,
                UserId = dto.UserId,
                Date = DateTime.Now
            };

            await _context.Sales.AddAsync(sales);
            await _context.SaveChangesAsync();

            dto.Id = sales.Id;
            return dto;
        }

        public async Task<SalesDto> CreateWithDetailsAsync(CreateSaleDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (dto.CustomerId <= 0)
                    throw new Exception("Cliente inválido.");

                if (dto.UserId <= 0)
                    throw new Exception("Usuario inválido.");

                if (dto.Details == null || dto.Details.Count == 0)
                    throw new Exception("La venta debe tener al menos un producto.");

                var sale = new Sales
                {
                    CustomerId = dto.CustomerId,
                    Total = (int)dto.Total,
                    UserId = dto.UserId,
                    Date = DateTime.Now
                };

                await _context.Sales.AddAsync(sale);
                await _context.SaveChangesAsync();

                foreach (var item in dto.Details)
                {
                    if (item.Quantity <= 0)
                        throw new Exception("La cantidad debe ser mayor que cero.");

                    var product = await _context.Products.FindAsync(item.ProductId);

                    if (product == null)
                        throw new Exception($"El producto con id {item.ProductId} no existe.");

                    if (product.Stock < item.Quantity)
                        throw new Exception($"Stock insuficiente para el producto {product.Name}.");

                    var detail = new SaleDetails
                    {
                        SaleId = sale.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = (int)item.Price
                    };

                    await _context.SaleDetails.AddAsync(detail);

                    product.Stock -= item.Quantity;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new SalesDto
                {
                    Id = sale.Id,
                    CustomerId = sale.CustomerId,
                    Total = sale.Total,
                    UserId = sale.UserId
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<SalesDto>> GetAllAsync()
        {
            return await _context.Sales
                .Select(s => new SalesDto
                {
                    Id = s.Id,
                    CustomerId = s.CustomerId,
                    Total = s.Total,
                    UserId = s.UserId
                })
                .ToListAsync();
        }

        public async Task<SalesDto?> GetByIdAsync(int id)
        {
            return await _context.Sales
                .Where(s => s.Id == id)
                .Select(s => new SalesDto
                {
                    Id = s.Id,
                    CustomerId = s.CustomerId,
                    Total = s.Total,
                    UserId = s.UserId,
                    Date = s.Date
                })
                .FirstOrDefaultAsync();
        }
    }
}