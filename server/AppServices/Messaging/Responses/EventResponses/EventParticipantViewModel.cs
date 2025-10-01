using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.Data.Enums;

namespace EventManager.AppServices.Messaging.Responses.EventResponses
{
    /// <summary>
    /// View модел, който описва участник в събитие и съдържа информация за неговия статус,
    /// както и за връзката му със събитието и останалите участници.
    /// </summary>
    public class EventParticipantViewModel
    {
        /// <summary>
        /// Уникален идентификатор на участието в събитието.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Събитието, към което е свързан участникът.
        /// </summary>
        public EventViewModel? Event { get; set; }

        /// <summary>
        /// Статус на поканата (напр. Поканен, Приет, Отказан).
        /// </summary>
        public InviteStatus Status { get; set; }

        /// <summary>
        /// Потребителят, който е поканен да участва в събитието.
        /// </summary>
        public UserViewModel? Invitee { get; set; }

        /// <summary>
        /// Потребителят, който е изпратил поканата.
        /// </summary>
        public UserViewModel? Inviter { get; set; }
    }
}
