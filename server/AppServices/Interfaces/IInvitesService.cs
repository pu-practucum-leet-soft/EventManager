namespace EventManager.AppServices.Interfaces
{
    using EventManager.AppServices.Messaging;
    using EventManager.AppServices.Messaging.Requests.InvitesRequests;
    using EventManager.AppServices.Messaging.Responses.InvitesResponses;

    /// <summary>
    /// Интерфейс, който дефинира бизнес логиката за работа с покани към събития.
    /// </summary>
    public interface IInvitesService
    {
        /// <summary>
        /// Зарежда всички входящи и изходящи покани за даден потребител.
        /// </summary>
        /// <param name="userId">Уникален идентификатор на потребителя.</param>
        /// <returns><see cref="GetInvitesAllResponse"/> с пълен списък от покани.</returns>
        Task<GetInvitesAllResponse> LoadAllInvitesAsync(Guid userId);

        /// <summary>
        /// Зарежда входящите покани за даден потребител (получени покани).
        /// </summary>
        /// <param name="req">Заявка съдържаща информация за потребителя и страниране.</param>
        /// <returns><see cref="GetInvitesIncomingResponse"/> с входящите покани.</returns>
        Task<GetInvitesIncomingResponse> LoadInvitesIncomingAsync(GetInvitesIncomingRequest req);

        /// <summary>
        /// Зарежда изходящите покани за даден потребител (изпратени покани).
        /// </summary>
        /// <param name="req">Заявка съдържаща информация за потребителя и страниране.</param>
        /// <returns><see cref="GetInvitesOutcomingResponse"/> с изходящите покани.</returns>
        Task<GetInvitesOutcomingResponse> LoadInvitesOutcomingAsync(GetInvitesOutcomingRequest req);

        /// <summary>
        /// Приема покана за дадено събитие.
        /// </summary>
        /// <param name="inviteId">Идентификатор на поканата.</param>
        /// <param name="actingUserId">Идентификатор на потребителя, който приема поканата.</param>
        /// <returns><see cref="ServiceResponseBase"/> със статус на операцията.</returns>
        Task<ServiceResponseBase> AcceptInviteAsync(Guid inviteId, Guid actingUserId);

        /// <summary>
        /// Отхвърля покана за дадено събитие.
        /// </summary>
        /// <param name="inviteId">Идентификатор на поканата.</param>
        /// <param name="actingUserId">Идентификатор на потребителя, който отхвърля поканата.</param>
        /// <returns><see cref="ServiceResponseBase"/> със статус на операцията.</returns>
        Task<ServiceResponseBase> DeclineInviteAsync(Guid inviteId, Guid actingUserId);

        /// <summary>
        /// Създава нова покана за събитие към избран потребител.
        /// </summary>
        /// <param name="eventId">Идентификатор на събитието.</param>
        /// <param name="inviterId">Идентификатор на потребителя, който изпраща поканата.</param>
        /// <param name="inviteeEmail">Имейл на потребителя, който трябва да получи поканата.</param>
        /// <returns><see cref="ServiceResponseBase"/> със статус на операцията.</returns>
        Task<ServiceResponseBase> CreateInviteAsync(Guid eventId, Guid inviterId, string inviteeEmail);

        /// <summary>
        /// Премахва участието на потребител от събитие (отказ от присъствие).
        /// </summary>
        /// <param name="eventId">Идентификатор на събитието.</param>
        /// <param name="userId">Идентификатор на потребителя, който се отписва.</param>
        /// <returns><see cref="ServiceResponseBase"/> със статус на операцията.</returns>
        Task<ServiceResponseBase> UnattendEventAsync(Guid eventId, Guid userId);
    }
}
