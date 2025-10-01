namespace EventManager.AppServices.Messaging
{
    /// <summary>
    /// Базов клас за всички отговори на услуги в приложението.
    /// Съдържа статус код, който описва резултата от изпълнението на заявката.
    /// </summary>
    public class ServiceResponseBase
    {
        /// <summary>
        /// Код на резултата, който указва дали операцията е успешна,
        /// неуспешна или е възникнала грешка.
        /// </summary>
        public BusinessStatusCodeEnum StatusCode { get; set; }

        /// <summary>
        /// Съобщение съдържащо повече информация за извършената операция и статуса ѝ.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Създава нов екземпляр на <see cref="ServiceResponseBase"/> 
        /// със стойност по подразбиране <see cref="BusinessStatusCodeEnum.None"/>.
        /// </summary>
        public ServiceResponseBase()
        {
            StatusCode = BusinessStatusCodeEnum.None;
        }

        /// <summary>
        /// Създава нов екземпляр на <see cref="ServiceResponseBase"/> със зададен статус код.
        /// </summary>
        /// <param name="statusCode">Статус код, описващ резултата от операцията.</param>
        public ServiceResponseBase(BusinessStatusCodeEnum statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
