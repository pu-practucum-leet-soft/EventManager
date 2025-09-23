using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        [Column("UserImage")]
        public string? UserImageUrl { get; set; }
    }
}
