namespace InventoryProBackend.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}
