namespace InventoryProBackend.Models
{
    public class Sales
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public int Total {  get; set; }
        public int UsuerId { get; set; }


    }
}
