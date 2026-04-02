namespace InventoryProBackend.Models
{
    public class Users
    {

      public int Id { get; set; }
        public string username { get; set; }
        public string PasswordHash { get; set; }
        public string RoleId { get; set; }
        public bool IsActive { get; set; }

    }
}
