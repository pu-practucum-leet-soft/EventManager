using System.Text.Json.Serialization;

namespace EventManager.AppServices.Messaging
{
    /// <summary>
    /// Клас за представяне на грешки при изпълнение на услуга.
    /// Наследява <see cref="ServiceResponseBase"/> и добавя информация,
    /// предназначена за потребителя и за разработчика.
    /// </summary>
    public class ServiceResponseError : ServiceResponseBase
    {
        /// <summary>
        /// Създава нов екземпляр на <see cref="ServiceResponseError"/> 
        /// със статус код <see cref="BusinessStatusCodeEnum.InternalServerError"/>.
        /// </summary>
        public ServiceResponseError() : base(BusinessStatusCodeEnum.InternalServerError) { }

        /// <summary>
        /// Подробно съобщение за грешка, предназначено за разработчици.
        /// Това свойство е игнорирано при сериализация към JSON, 
        /// за да не се излага чувствителна информация.
        /// </summary>
        [JsonIgnore]
        public string? DeveloperError { get; set; }

        /// <summary>
        /// Съобщение за грешка, което може да бъде показано на потребителя.
        /// </summary>
        public new string? Message { get; set; }
    }
}
