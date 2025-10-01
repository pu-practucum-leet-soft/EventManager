using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Data.Entities
{
    /// <summary>
    /// Представлява потребител в системата.
    /// Наследява <see cref="IdentityUser{Guid}"/> и добавя допълнителни свойства,
    /// специфични за приложението.
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        /// <summary>
        /// URL адрес към профилната снимка на потребителя.
        /// Може да сочи към локален ресурс или външна услуга за съхранение на изображения.
        /// </summary>
        [Column("UserImage")]
        public string? UserImageUrl { get; set; }
    }
}
