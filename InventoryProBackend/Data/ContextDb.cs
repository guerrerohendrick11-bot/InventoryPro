using Microsoft.EntityFrameworkCore;

namespace InventoryProBackend.Data
{
    public class ContextDb: DbContext
    {
        public ContextDb(DbContextOptions<ContextDb>options): base(options)
        {
            

        }
         public DbSet<Models.Categories> Categories { get; set; }
         public DbSet<Models.Customers> Customers{ get; set; }
         public DbSet<Models.Products> Products { get; set; }
         public DbSet<Models.Roles> Roles { get; set; }
         public DbSet<Models.SaleDetails> SaleDetails { get; set; }
         public DbSet<Models.Sales> Sales { get; set; }
         public DbSet<Models.Users> Users { get; set; }
       
    }
}
