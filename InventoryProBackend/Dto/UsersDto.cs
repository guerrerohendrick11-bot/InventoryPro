namespace InventoryProBackend.Dto
{
    public class UsersDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}
