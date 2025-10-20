namespace inventoryApiDotnet.Model
{
    public class User
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string Role { get; set; } = "Staff";
    }
}