using System.ComponentModel.DataAnnotations;

namespace EventManager.Data.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Role { get; set; } // user или admin
        [Required]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? UserImageUrl { get; set; }
    }
}
