using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.Data.Entities;

namespace EventManager.AppServices.Messaging.Responses.InvitesResponses
{
    /// <summary>
    /// ViewModel, който представя информация за покана към дадено събитие.
    /// Използва се за визуализация и трансфер на данни между бекенд и клиент.
    /// </summary>
    public class InviteViewModel
    {
        /// <summary>
        /// Уникален идентификатор на събитието, към което е изпратена поканата.
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Заглавието на събитието.
        /// </summary>
        public string? EventTitle { get; set; }

        /// <summary>
        /// Потребителят, който е изпратил поканата (инициатор).
        /// </summary>
        public UserViewModel? Inviter { get; set; }

        /// <summary>
        /// Потребителят, който получава поканата.
        /// </summary>
        public UserViewModel? Invitee { get; set; }

        /// <summary>
        /// Флаг, указващ дали потребителят се е присъединил успешно към събитието.
        /// </summary>
        public bool Join { get; set; }

        /// <summary>
        /// Текущият статус на поканата (поканен, приет, отказан).
        /// </summary>
        public Data.Enums.InviteStatus InviteStatus { get; set; }
    }
}
