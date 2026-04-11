namespace InventoryProBackend.Dto
{
    public class CreateSaleDto
    {
        public int CustomerId { get; set; }
        public decimal Total { get; set; }
        public int UserId { get; set; }
        public List<CreateSaleDetailDto> Details { get; set; } = new();
    }
}