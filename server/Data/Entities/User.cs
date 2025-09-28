using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        [Column("UserImage")]
        public string? UserImageUrl { get; set; }
    }
}
