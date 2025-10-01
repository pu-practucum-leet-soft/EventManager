namespace EventManager.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Представлява refresh token за потребител,
    /// използван за издаване на нов JWT токен след изтичане на стария.
    /// Съдържа информация за срока на валидност, IP адреса при създаване и евентуално отнемане.
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// Уникален идентификатор на refresh токена.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Стойността на refresh токена (уникален низ, съхраняван сигурно).
        /// </summary>
        [Required]
        public string Token { get; set; } = default!;

        /// <summary>
        /// Идентификатор на потребителя, към когото принадлежи токена.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Навигационно свойство към потребителя.
        /// </summary>
        public User? User { get; set; }

        /// <summary>
        /// Време на изтичане на токена.
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// Дата и час на създаване на токена.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// IP адресът, от който е създаден токена.
        /// </summary>
        public string CreatedByIp { get; set; } = default!;

        /// <summary>
        /// Дата и час на отнемане (ревокация) на токена.
        /// </summary>
        public DateTime? Revoked { get; set; }

        /// <summary>
        /// IP адресът, от който е направена ревокацията.
        /// </summary>
        public string? RevokedByIp { get; set; }

        /// <summary>
        /// Проверява дали токенът е изтекъл.
        /// </summary>
        [NotMapped]
        public bool IsExpired => DateTime.UtcNow >= Expires;

        /// <summary>
        /// Проверява дали токенът е активен (не е изтекъл и не е отнет).
        /// </summary>
        [NotMapped]
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
