namespace InventoryProBackend.Dto
{
	public class SaleDetailsDto
	{
		public int Id { get; set; }
		public int SaleId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
    }
}

 