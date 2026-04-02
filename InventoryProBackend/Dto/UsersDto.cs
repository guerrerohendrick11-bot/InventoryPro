namespace InventoryProBackend.Dto
{
    public class UsersDto
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string PasswordHash { get; set; }
        public string RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}
