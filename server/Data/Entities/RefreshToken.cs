namespace EventManager.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Token { get; set; } = default!;
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public User? User { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Expires { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Created { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// 
        /// </summary>
        public string CreatedByIp { get; set; } = default!;
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Revoked { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? RevokedByIp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public bool IsExpired => DateTime.UtcNow >= Expires;
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public bool IsActive => Revoked == null && !IsExpired;
    }

}
