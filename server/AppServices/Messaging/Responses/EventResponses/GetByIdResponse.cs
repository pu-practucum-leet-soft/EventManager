namespace EventManager.AppServices.Messaging.Responses.EventResponses
{
    /// <summary>
    /// Отговорът при заявка за конкретно събитие по неговия идентификатор.
    /// Наследява <see cref="ServiceResponseBase"/> и съдържа данни за събитието.
    /// </summary>
    public class GetByIdResponse : ServiceResponseBase
    {
        /// <summary>
        /// Данните за намереното събитие.
        /// </summary>
        public EventViewModel Event { get; set; } = default!;
    }
}
