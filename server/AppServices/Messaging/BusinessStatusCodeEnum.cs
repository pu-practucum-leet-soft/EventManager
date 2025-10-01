namespace EventManager.AppServices.Messaging
{
    /// <summary>
    /// Бизнес статус кодове, използвани за унифицирано
    /// обозначаване на резултатите от изпълнение на заявки и операции.
    /// </summary>
    public enum BusinessStatusCodeEnum
    {
        /// <summary>
        /// Няма зададен статус.
        /// </summary>
        None,

        /// <summary>
        /// Операцията е изпълнена успешно.
        /// </summary>
        Success,

        /// <summary>
        /// Заявеният ресурс не е намерен.
        /// </summary>
        NotFound,

        /// <summary>
        /// Възникна конфликт – например опит за създаване на вече съществуващ ресурс.
        /// </summary>
        Conflict,

        /// <summary>
        /// Възникна вътрешна грешка в сървъра.
        /// </summary>
        InternalServerError,

        /// <summary>
        /// Достъпът е отказан поради липса на автентикация или права.
        /// </summary>
        Unauthorized,

        /// <summary>
        /// Подадената заявка е невалидна или непълна.
        /// </summary>
        BadRequest,
        /// <summary>
        /// 
        /// </summary>
        Forbidden
    }
}
