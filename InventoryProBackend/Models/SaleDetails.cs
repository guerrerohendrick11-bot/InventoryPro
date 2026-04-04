namespace InventoryProBackend.Models
{
    public class SaleDetails
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int SudTotal { get; set; }
    }
}
