using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventoryApiDotnet.Model
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string Role { get; set; } = "Staff";
    }
}